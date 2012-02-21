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
        public List<Nut> removeNutList; //remove nuts
        public List<Nut> currentNutList; //generate nuts
        public uint level = 0;
        Random rand; //random number generator
        float nutAcceleration = 50f;
        float nutFrequency = 2.0f;
        float levelFrequency = 60f;
        GameTimer nutsFalling, levelTimer;

        //must pass lanewidth from Game1 object
        public GameBoard( int displayWidth ) {
            int laneWidth = displayWidth / ( laneCount );
            //set up random numbers
            this.rand = new Random();

            this.Lanes = new int[7];
            for( int i = 0; i < laneCount; i++ ) {
                this.Lanes[i] = i * laneWidth;
            }

            this.currentNutList = new List<Nut>();
            this.removeNutList = new List<Nut>();
            //this.currentPineCones = new List<PineCone>();
            //this.removePineCones = new List<PineCone>();

            //sets up event, to affect frequency of nuts, adjust update interval...perhaps add randomness?
            this.nutsFalling = new GameTimer();
            nutsFalling.UpdateInterval = TimeSpan.FromSeconds(nutFrequency);
            nutsFalling.Update += new EventHandler<GameTimerEventArgs>(generateNuts);
            nutsFalling.Start();

            //sets up level up timer
            this.levelTimer = new GameTimer();
            levelTimer.UpdateInterval = TimeSpan.FromSeconds(levelFrequency);
            levelTimer.Update += new EventHandler<GameTimerEventArgs>(levelUp);
            levelTimer.Start();
        }

        void generateNuts(object sender, GameTimerEventArgs e)
        {
            int dropLane = rand.Next(0, 7);
            int pineCheck = rand.Next(0, 7);
            if (pineCheck > 4)
                currentNutList.Add(new Nut(20, Lanes[dropLane], true, nutAcceleration));
            else
                currentNutList.Add(new Nut(20, Lanes[dropLane], false, nutAcceleration));
        }

        //fire if the nut is x > height of the box and the nut's y is within the width of the box
        public void nutCatch( Vector2 heroLocation, ref uint score, ref uint lives )
        {
            if ( this.currentNutList.Count > 0)
            {
                foreach (Nut nt in this.currentNutList)
                {
                    //if its a pinecone, decrement lives
                    if (nt.badForYou)
                    {
                        if (nt.Position.X > heroLocation.X - 35 && //top check
                        nt.Position.X < heroLocation.X + 120 && //bottom check
                        nt.Position.Y < heroLocation.Y + 55 && //left check
                        nt.Position.Y > heroLocation.Y - 35) //right check
                        {
                            this.removeNutList.Add(nt);
                            lives--;
                        } //end if
                    }
                    //if its a nut, inc. score
                    else
                    {
                        if (nt.Position.X > heroLocation.X - 50 && //top check
                        nt.Position.X < heroLocation.X + 120 && //bottom check
                        nt.Position.Y < heroLocation.Y + 65 && //left check
                        nt.Position.Y > heroLocation.Y - 35) //right check
                        {
                            this.removeNutList.Add(nt);
                            score++;
                        } //end if
                    }
                } //end for
            } //end if
            foreach (Nut nt in this.removeNutList)
            {
                this.currentNutList.Remove(nt);
            }
        } //end nutCatch

        //move nuts
        public void moveNuts(GameTime gameTime, int MaxX )
        {
            foreach (Nut nt in currentNutList)
            {
                nt.Speed.X += (nt.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
                nt.Position += nt.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentNutList.Count > 0 && nt.Position.X > MaxX)
                {
                    removeNutList.Add(nt);
                }
            }
        }

        public void removeNuts()
        {
            //remove nuts that have fallen off screen
            foreach (Nut nt in removeNutList)
            {
                currentNutList.Remove(nt);
            }
        }

        void levelUp(object sender, GameTimerEventArgs e)
        {
            level++;
            nutAcceleration *= 1.2f;
            nutFrequency *= 0.75f;
            nutsFalling.UpdateInterval = TimeSpan.FromSeconds(nutFrequency);
            //implement tick-tock
            //increase drop frequency
            //increase nut speed
            //increase pinecone:nut ratio
        }
    }
}
