using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NutsForNutsGAME
{
    class Sprite
    {
        private Texture2D texture;
        private int width, height, padX, padY, frames;
        private int counter, timeout, timeoutCounter = 0;
        private bool bRepeating = true, bReverse = false;
        private Rectangle rect = new Rectangle();
        private float rotation = 0f, scale = 1f;
        private Vector2 origin;


        public Sprite(Texture2D texture, int frames, int timeout, int width, int height, int padX, int padY)
        {
            this.texture = texture;
            this.frames = frames;
            this.timeout = timeout;
            this.width = width;
            this.height = height;
            this.padX = padX;
            this.padY = padY;
            
            origin = new Vector2(width / 2, height / 2);
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public void SetScale(float scale)
        {
            this.scale = scale;
        }

        public void Progress(int elapsed)
        {
            if (frames < 1)
                return;

            timeoutCounter += elapsed;

            if (timeoutCounter >= timeout)
            {
                if (bReverse)
                {
                    if (!bRepeating && counter == 0)
                        return;

                    counter = (counter - 1) % frames;
                }
                else
                {
                    if (!bRepeating && counter == frames)
                        return;

                    counter = (counter + 1) % frames;
                }
                
                timeoutCounter -= timeout;
            }
        }

        public bool IsRepeating()
        {
            return bRepeating;
        }

        public bool IsReverse()
        {
            return bReverse;
        }

        public bool IsAnimated()
        {
            return frames > 1;
        }

        public void Draw(SpriteBatch batch, Vector2 screenpos, float floatY, float depth)
        {
            Draw(batch, counter, screenpos, 0, floatY, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 screenpos, int objHeight, float floatY, float depth)
        {
            Draw(batch, counter, screenpos, objHeight, floatY, depth);
        }

        public void Draw(SpriteBatch batch, int frame, Vector2 screenpos, int objHeight, float floatY, float depth)
        {
            rect.X = width * counter;
            rect.Y = 0;
            rect.Width = width;
            rect.Height = height;

            screenpos.X -= padX;
            screenpos.Y -= objHeight + padY;

            batch.Draw(texture, screenpos, rect, Color.White, rotation, origin, scale, SpriteEffects.None, depth);
        }
    }
}
