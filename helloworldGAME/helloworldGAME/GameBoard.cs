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
using helloworldGAME;

namespace helloworldGAME
{
    class GameBoard
    {
        public int[] Lanes; //contains pixel info for invisible lanes
        public int laneCount = 7; //number of lanes
        public List<Nut> removeNuts;//remove nuts
        public List<Nut> currentNuts;//generate nuts
        public List<PineCone> removePineCones;//remove pinecones
        public List<PineCone> currentPineCones;//generate pinecones
        Random rand; //random number generator

        //must pass lanewidth from Game1 object
        public GameBoard( int displayWidth ) {
            int laneWidth = displayWidth / ( laneCount );
            //set up random numbers
            this.rand = new Random();

            this.Lanes = new int[7];
            for( int i = 0; i < laneCount; i++ ) {
                this.Lanes[i] = i * laneWidth;
            }

            this.currentNuts = new List<Nut>();
            this.removeNuts = new List<Nut>();
            this.currentPineCones = new List<PineCone>();
            this.removePineCones = new List<PineCone>();

            //sets up event, to affect frequency of nuts, adjust update interval...perhaps add randomness?
            GameTimer nutsFalling = new GameTimer();
            nutsFalling.UpdateInterval = TimeSpan.FromSeconds(2);
            nutsFalling.Update += new EventHandler<GameTimerEventArgs>(generateNuts);
            nutsFalling.Start();
        }

        void generateNuts(object sender, GameTimerEventArgs e)
        {
            int dropLane = rand.Next(0, 7);
            for (int i = 0; i < 7; i++)
            {
                currentNuts.Add(new Nut(20, Lanes[i]));
            }
        }

        //fire if the nut is x > height of the box and the nut's y is within the width of the box
        public void nutCatch( Vector2 heroLocation, ref uint score )
        {
            if ( this.currentNuts.Count > 0)
            {
                foreach (Nut nt in this.currentNuts)
                {
                    if (nt.Position.X > heroLocation.X - 50 && //top check
                        nt.Position.X < heroLocation.X + 120 && //bottom check
                        nt.Position.Y < heroLocation.Y + 65 && //left check
                        nt.Position.Y > heroLocation.Y - 35) //right check
                    {

                        this.removeNuts.Add(nt);
                        score++;
                    } //end if
                } //end for
            } //end if
            foreach (Nut nt in this.removeNuts)
            {
                this.currentNuts.Remove(nt);
            }
        } //end nutCatch

        
    }
}
