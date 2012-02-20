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

        public Hero()
        {
            this.MaxY = 400; //graphics.GraphicsDevice.Viewport.Height - animation_Feet.FrameWidth / 2;
            this.MinY = 0;
            setSpeed( new Vector2(0f, 500f) );
            this.location.X = 100;
            this.destination.X = 100;
            setLocation( 250f );
            setDestination( 250f );
        }
        public void move( GameTime gameTime )
        {
            //find out where we're going
            float dest = UI.getTouch().Y;
            System.Diagnostics.Debug.WriteLine("touched Y: " + UI.getTouch().Y);
            setDestination( dest ); //fixxxxxxx 50 to general case
            System.Diagnostics.Debug.WriteLine("new dest: " + this.destination.Y );
            if (this.destination.Y > this.MaxY)
            {
                System.Diagnostics.Debug.WriteLine("maxy is " + this.MaxY);
                setDestination(this.MaxY);
            }
            else if (this.destination.Y < this.MinY)
            {
                System.Diagnostics.Debug.WriteLine("miny is " + this.MinY);
                setDestination(this.MinY);
            }

            //ok, gotten our destination, now let's move the hero
            System.Diagnostics.Debug.WriteLine("dest: " + this.destination.Y + " loc: " + this.location.Y );
            // Move the hero location by speed, scaled by elapsed time.
            if ( this.destination.Y < ( this.location.Y + 20f) && 
                 this.destination.Y > ( this.location.Y - 20f) )
            {
                setLocation( this.destination.Y );
            }
            else if ( this.destination.Y > this.location.Y )
            {
                //animation_Feet.WalkDirection = true;

                int newY = (int)( this.location.Y + ( getSpeed().Y * (float)( gameTime.ElapsedGameTime.TotalSeconds )));
                System.Diagnostics.Debug.WriteLine("new Y = " + newY );
                setLocation( newY );
            }
            else if ( this.destination.Y < this.location.Y )
            {
                int newY = (int)( this.location.Y - ( getSpeed().Y * (float)gameTime.ElapsedGameTime.TotalSeconds ));
                //animation_Feet.WalkDirection = false;
                System.Diagnostics.Debug.WriteLine("new Y = " + newY);
                setLocation( newY );
            }//done moving hero 
            System.Diagnostics.Debug.WriteLine("postmove location: " + this.location.Y );
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