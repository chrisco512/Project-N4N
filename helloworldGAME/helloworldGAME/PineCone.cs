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
    public class PineCone : Nut
    {
        public PineCone(float x, float y) : base( x, y) 
        {
            Position = new Vector2(0, y);
            Speed = new Vector2(0f, 0f);
        }
    }
}