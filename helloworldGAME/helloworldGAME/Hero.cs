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
    class Hero
    {
        private Vector2 destination;
        private Vector2 location;
        private Vector2 speed;
        //for boundary constraints
        private int MaxY;
        private int MinY;
        private int MaxX;
        private int MinX;

        public Hero(int MaxX, int MaxY)
        {
            this.MaxY = MaxY; //graphics.GraphicsDevice.Viewport.Height - animation_Feet.FrameWidth / 2;
            this.MinY = 0;
            this.MaxX = MaxX;
            this.MinX = 0;
            setSpeed( new Vector2(0f, 1000f) );
            this.location.X = MaxX - 120 - 100;
            this.destination.X = this.location.X;
            this.location.Y = MaxY / 2;
            this.destination.Y = this.location.Y;
            //setLocation( 250f );
            //setDestination( 250f );
        }
        public void move( GameTime gameTime )
        {
            //find out where we're going
            float dest = UI.getTouch().Y - 50;
            setDestination( dest ); //fixxxxxxx 50 to general case
            if (this.destination.Y > this.MaxY)
            {
                setDestination(this.MaxY);
            }
            else if (this.destination.Y < this.MinY)
            {
                setDestination(this.MinY);
            }

            //ok, gotten our destination, now let's move the hero
            //System.Diagnostics.Debug.WriteLine("dest: " + this.destination.Y + " loc: " + this.location.Y );
            // Move the hero location by speed, scaled by elapsed time.
            if ( this.destination.Y < ( this.location.Y + 20f) && 
                 this.destination.Y > ( this.location.Y - 20f) )
            {
                setLocation( this.destination.Y );
            }
            else if ( this.destination.Y > this.location.Y )
            {
                int newY = (int)( this.location.Y + ( getSpeed().Y * (float)( gameTime.ElapsedGameTime.TotalSeconds )));
                setLocation( newY );
            }
            else if ( this.destination.Y < this.location.Y )
            {
                int newY = (int)( this.location.Y - ( getSpeed().Y * (float)gameTime.ElapsedGameTime.TotalSeconds ));
                setLocation( newY );
            } //done moving hero
        } // end move


        //getters and setters
        public Vector2 getDestination()
        {
            return this.destination;
        }
        public Vector2 getLocation()
        {
            return this.location;
        }
        public Vector2 getSpeed()
        {
            return this.speed;
        }
        public void setDestination( float destination )
        {
            this.destination.Y = destination;
        }
        public void setLocation( float location )
        {
            this.location.Y = location;
        }
        public void setSpeed( Vector2 speed )
        {
            this.speed = speed;
        }
        public void setMaxY( int MaxY )
        {
            this.MaxY = MaxY;
        }

    } //end hero
}//end namespace