﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    class SpeedPowerUp: PowerUp
    {
        public static int SPEED_POWERUP = 0;

        public SpeedPowerUp(Texture2D newTexture, Vector2 newPos)
            : base(newTexture, newPos)
        { }
        
       public override int getPowerUp() //using traditional getter for this, faster than c# ones in this case
    {
        return SPEED_POWERUP;
    }
    }
}
