using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace helloworldGAME
{
    public class SquirrelAnimation
    {
        Texture2D spriteStrip;
        //scale used to display the sprite strip
        float scale;
        //the time since we last updated the frame
        int elapsedTime;
        //time we display a frame until next one
        int frameTime;
        //number of frames that the animation contains
        int frameCount;
        //the index of the current frame we are displaying
        int currentFrame;
        //the color of the frame we will be displaying
        Color color;
        //the area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();
        //the area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();
        //width of a given frame
        public int FrameWidth;
        public int FrameHeight;
        //the state of the animation
        public bool Active;
        //determines if the animation will keep playing or deactivate after one run
        public bool Looping;
        //set walking direction, 0 for left, 1 for right
        public bool WalkDirection;
        //Position of the box to animate
        public Vector2 Position;
        public void Initialize(Texture2D texture, Vector2 position,
                                int frameWidth, int frameHeight, int frameCount,
                                int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            Looping = looping;
            Position = position;
            spriteStrip = texture;
            //set the time to zero
            elapsedTime = 0;
            currentFrame = 0;
            //set the animation to active by default
            Active = true;
        }
        public void Update(GameTime gameTime)
        {
            if (Active == false) return;
            //update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            //if the elapsed time is larger than the frame time, switch frames
            if (elapsedTime > frameTime)
            {
                //move to the next frame
                if (Looping == true)
                {
                    if (WalkDirection == false)
                        currentFrame--;
                    else
                        currentFrame++;
                }
                else
                    currentFrame = 0;
                //if the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    //if we are not looping deactivate the animation
                    //if (Looping == false) { }
                        //Active = false;
                }
                if (currentFrame < 0)
                {
                    currentFrame = frameCount - 1;
                }
                //reset the elapsed time to zero
                elapsedTime = 0;
            }
            //grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(0, currentFrame * FrameHeight, FrameHeight, FrameWidth);
            //grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = //new Rectangle((int)Position.X, (int)Position.Y, (int)FrameHeight, (int)FrameWidth);
                
                new Rectangle( (int)Position.X,//(int)Position.X, // /2 
                                (int)Position.Y,//(int)Position.Y,
                                (int)(FrameWidth * scale),
                                (int)(FrameHeight * scale));

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //onlly draw the animation when we are active
            if (Active)
            {
                System.Diagnostics.Debug.WriteLine("srcrect: " + sourceRect + " frame: " + currentFrame);
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                //spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, Color.White, (float)(-Math.PI / 2), new Vector2(120/2, 100/2), SpriteEffects.None, 0);
            }
        }
    }
}
