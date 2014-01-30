using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    abstract class Powerup
    {
       protected int x;
       protected int y;

        public Powerup(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Draw(SpriteBatch spritebatch);
        


    }
}
