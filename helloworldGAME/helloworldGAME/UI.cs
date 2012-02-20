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

public class UI
{
    public static Vector2 currentLocation;
    public static Vector2 getTouch()
    {
        TouchCollection touchCollection = TouchPanel.GetState();
        foreach (TouchLocation tl in touchCollection)
        {
            if (tl.State == TouchLocationState.Moved)
            {
                currentLocation = tl.Position;
            }
        }
        return currentLocation;
    }
}