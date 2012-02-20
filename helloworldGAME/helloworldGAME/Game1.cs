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
        int displayWidth;

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

        Hero squirrel;

        //for drawing background to screen against this rectangle
        Rectangle bgRect;
        
        //for reading input
        private Sprite squirrel_feet;
        private Sprite squirrel_body;
        private Sprite nut_image;
            
        //constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

           

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            squirrel = new Hero(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            displayWidth = graphics.GraphicsDevice.Viewport.Height;
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
            scoreText = Content.Load<SpriteFont>("ComicSans");         
            fontPos = new Vector2( 20f, 20f);

            //animation textures
            squirrelTexture_Feet = Content.Load<Texture2D>("new_squirrel_down");
            ImageLibrary.ConvertTransparentPixels(squirrelTexture_Feet);
            squirrel_feet = new Sprite(squirrelTexture_Feet, 8, 1, 100, 120, 0, 0);
            squirrel_feet.SetRotation((float)(-Math.PI / 2));

            squirrelTexture_Body = Content.Load<Texture2D>("new_squirrel_up");
            ImageLibrary.ConvertTransparentPixels(squirrelTexture_Body);
            squirrel_body = new Sprite(squirrelTexture_Body, 8, 1, 100, 120, 0, 0);
            squirrel_body.SetRotation((float)(-Math.PI / 2));

            nutTexture_L = Content.Load<Texture2D>("nut");
            ImageLibrary.ConvertTransparentPixels(nutTexture_L);
            nut_image = new Sprite(nutTexture_L, 1, 1, 70, 70, 0, 0);

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
                nt.Speed.X += (3200 * (float)gameTime.ElapsedGameTime.TotalSeconds);
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

            // check for catches
            // Move the sprite around.
            squirrel.move( gameTime );
            System.Diagnostics.Debug.WriteLine("squirrelPos: " + squirrel.getLocation().Y + " goToPos: " + squirrel.getDestination().X + "," + squirrel.getDestination().Y + " spriteSpreed: " + squirrel.getSpeed() );
            board.nutCatch( squirrel.getLocation(), ref score);
            squirrel_feet.Progress(1);
            squirrel_body.Progress(1);
            base.Update(gameTime);
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
            squirrel_feet.Draw(spriteBatch, new Vector2( squirrel.getLocation().X + 60, squirrel.getLocation().Y + 50), 0, 0 );
            squirrel_body.Draw(spriteBatch, new Vector2(squirrel.getLocation().X + 60, squirrel.getLocation().Y + 50), 0, 0);
            ImageLibrary.DrawRectangle(graphics.GraphicsDevice, spriteBatch, new Rectangle((int)squirrel.getLocation().X, (int)squirrel.getLocation().Y, 120, 100), Color.White);
            spriteBatch.End();

            foreach( Nut nt in board.currentNuts ) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                nut_image.Draw(spriteBatch, new Vector2(nt.Position.X + 36, nt.Position.Y + 36), 0, 0);
                //spriteBatch.Draw(nutTexture_L, nt.Position, Color.White);
                ImageLibrary.DrawRectangle(graphics.GraphicsDevice, spriteBatch, new Rectangle( (int)nt.Position.X, (int)nt.Position.Y, nutTexture_L.Height, nutTexture_L.Width), Color.White);
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
