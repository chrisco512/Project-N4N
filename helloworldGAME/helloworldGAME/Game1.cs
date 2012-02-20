using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using helloworldGAME;


namespace NutsForNutsGAME
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GameBoard board;

        /* Declare variables */
            //for onscreen score
            uint score = 0;
            SpriteFont scoreText;
            Vector2 fontPos;

            //for graphics control
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;

            //set up sprite textures
            Texture2D squirrelTexture_Feet;
            Texture2D squirrelTexture_Body;
            Texture2D nutTexture_L;
            Texture2D bgTexture;
            SquirrelAnimation animation_Feet;
            SquirrelAnimation animation_Body;

            //for drawing background to screen against this rectangle
            Rectangle bgRect;

            //squirrel's position and speed vectors
            //Vector2 squirrelPos;
            Vector2 squirrelSpeed = new Vector2(0f, 500f);
        
            //random number generator
            //Random rand;
        
            //the Y value of the last pointed to position by the user
            float goToPos;
        
            //for reading input
            public TouchCollection TouchState;
            public readonly List<GestureSample> Gestures = new List<GestureSample>();
            private Sprite squirrel_feet;
            
        //constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
            //rand = new Random();

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            //board.currentNuts = new List<Nut>();
            //board.removeNuts = new List<Nut>();

            //sets up event, to affect frequency of nuts, adjust update interval...perhaps add randomness?
            //GameTimer nutsFalling = new GameTimer();
            //nutsFalling.UpdateInterval = TimeSpan.FromSeconds(2);
            //nutsFalling.Update += new EventHandler<GameTimerEventArgs>(nutGenerator);
            //nutsFalling.Start();

        }

        //creates new nuts
        //public void nutGenerator(object sender, GameTimerEventArgs e)
        //{
        //    board.currentNuts.Add(new Nut(20, rand.Next(0, graphics.GraphicsDevice.Viewport.Height)));
        //}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            animation_Feet = new SquirrelAnimation();
            animation_Body = new SquirrelAnimation();
            int displayWidth = graphics.GraphicsDevice.Viewport.Height;
            board = new GameBoard(displayWidth);

            
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //score style and location
            scoreText = Content.Load<SpriteFont>("Arial");         
            fontPos = new Vector2( 20f, 20f);

            //animation textures
            squirrelTexture_Feet = Content.Load<Texture2D>("new_squirrel_down");
            ImageLibrary.ConvertTransparentPixels(squirrelTexture_Feet);
            squirrel_feet = new Sprite(squirrelTexture_Feet, 8, 1, 100, 120, 0, 0);
            squirrel_feet.SetRotation((float)(-Math.PI / 2));

            squirrelTexture_Body = Content.Load<Texture2D>("new_squirrel_up");
            animation_Feet.Initialize(squirrelTexture_Feet, new Vector2(graphics.GraphicsDevice.Viewport.Width - 120 - 100, 250),
                                  120, 100, 8, 20, Color.White, 1f, true);
            animation_Body.Initialize(squirrelTexture_Body, new Vector2(graphics.GraphicsDevice.Viewport.Width - 120 - 100, 250),
                                  120, 100, 8, 20, Color.White, 1f, true);

            nutTexture_L = Content.Load<Texture2D>("nut");
            bgTexture = Content.Load<Texture2D>("othertree");
            bgRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allow the game to exit.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            //move nuts on screen
            foreach (Nut nt in board.currentNuts)
            {
                nt.Speed.X += (50 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                nt.Position += nt.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (board.currentNuts.Count > 0 && nt.Position.X > graphics.GraphicsDevice.Viewport.Width)
                {
                    board.removeNuts.Add(nt);
                }
            }

            //remove nuts that have fallen off screen
            foreach( Nut nt in board.removeNuts ) {
                board.currentNuts.Remove( nt );
            }

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Moved)
                {
                    goToPos = tl.Position.Y - animation_Feet.FrameWidth/2;

                    int MaxY = graphics.GraphicsDevice.Viewport.Height - animation_Feet.FrameWidth/2;
                    int MinY = 0;

                    //  Check for bounce.
                    if (goToPos > MaxY)
                    {
                        goToPos = MaxY;
                    }
                    else if ( goToPos < MinY)
                    {
                        goToPos = MinY;
                    }
                }
                
            }
            // check for catches
            nutCatch();
            // Move the sprite around.
            UpdateSprite(gameTime, ref animation_Feet.Position, ref squirrelSpeed);
            //CheckForCollision();

            animation_Feet.Update(gameTime);
            animation_Body.Update(gameTime);
            squirrel_feet.Progress(1);
            base.Update(gameTime);
        }
        
        //fire if the nut is x > height of the box and the nut's y is within the width of the box
        void nutCatch()
        {
            if( board.currentNuts.Count > 0 ) {
                foreach (Nut nt in board.currentNuts)
                {
                    if (nt.Position.X > animation_Feet.Position.X - nutTexture_L.Width && nt.Position.X < animation_Feet.Position.X + animation_Feet.FrameHeight)
                    {
                        if (nt.Position.Y < animation_Feet.Position.Y + animation_Feet.FrameWidth + nutTexture_L.Height && nt.Position.Y > animation_Feet.Position.Y - nutTexture_L.Height)
                        {
                            board.removeNuts.Add(nt);
                            score++;
                        }
                    }
                }
                
            }
            foreach (Nut nt in board.removeNuts)
            {
                board.currentNuts.Remove(nt);
            }
        }

        void UpdateSprite(GameTime gameTime, ref Vector2 squirrelPos, ref Vector2 spriteSpeed)
        {
            if (squirrelPos.Y == goToPos)
                animation_Feet.Looping = false;
            else
                animation_Feet.Looping = true;
            System.Diagnostics.Debug.WriteLine("squirrelPos: " + squirrelPos.Y + " goToPos: " + goToPos + " spriteSpreed: " + spriteSpeed);
            // Move the sprite by speed, scaled by elapsed time.
            //squirrelPos +=
            //    spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (squirrelPos.Y < (goToPos + 20f) && squirrelPos.Y > (goToPos - 20f))
            {
                //animation.Looping = false;
                squirrelPos.Y = goToPos;
            }
            else if (squirrelPos.Y  > goToPos)
            {
                animation_Feet.WalkDirection = true;
                //animation.Looping = true;
                squirrelPos += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (squirrelPos.Y  < goToPos)
            {
                animation_Feet.WalkDirection = false;
                //animation.Looping = true;
                squirrelPos -= spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } 
            

            int MaxX =
                graphics.GraphicsDevice.Viewport.Width - animation_Feet.FrameHeight;
            int MinX = 0;
            int MaxY =
                graphics.GraphicsDevice.Viewport.Height - animation_Feet.FrameWidth;
            int MinY = 0;

           //  Check for bounce.
            if (squirrelPos.X > MaxX)
            {
                spriteSpeed.X *= -1;
                squirrelPos.X = MaxX;
            }

            else if (squirrelPos.X < MinX)
            {
                spriteSpeed.X *= -1;
                squirrelPos.X = MinX;
            }

            if (squirrelPos.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                squirrelPos.Y = MaxY;
            }

            else if (squirrelPos.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                squirrelPos.Y = MinY;
            }

            animation_Body.Position.X = squirrelPos.X;
            animation_Body.Position.Y = squirrelPos.Y;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(bgTexture, bgRect, Color.White);
            spriteBatch.End();
            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Draw(squirrelTexture_Main, squirrelPos, Color.White);
            animation_Feet.Draw(spriteBatch);
            animation_Body.Draw(spriteBatch);
            squirrel_feet.Draw(spriteBatch, new Vector2(100, 100), 0, 0);
            ImageLibrary.DrawRectangle(graphics.GraphicsDevice, spriteBatch, new Rectangle(100, 100, 1, 1), Color.Red);
            spriteBatch.End();

            foreach( Nut nt in board.currentNuts ) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(nutTexture_L, nt.Position, Color.White);
                spriteBatch.End();
            }
            //BEGIN score draw
                spriteBatch.Begin();
                // score string
                string output = "" + score;
                // Find the center of the string
                Vector2 FontOrigin = scoreText.MeasureString(output) / 2;
                fontPos.Y = scoreText.MeasureString(output).Y + 10;
                // Draw the string
                spriteBatch.DrawString(scoreText, output, fontPos, Color.Red,
                    -1.57f, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            //END score draw
            base.Draw(gameTime);

        }

    }
}
