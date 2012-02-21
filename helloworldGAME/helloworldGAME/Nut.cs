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
    /* contains position and speed information for each nut created on screen */
    public class Nut
    {
        public Vector2 Position;
        public Vector2 Speed;
        public float Acceleration;
        public bool badForYou;

        public Nut(float x, float y, bool badForYou)
        {
            Position = new Vector2(0, y);
            Speed = new Vector2(0f, 0f);
            this.badForYou = badForYou;
            Acceleration = 400f;
        }
    }
}