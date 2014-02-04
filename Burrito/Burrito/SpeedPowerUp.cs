using System;
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
        Texture2D myTexture;
        public SpeedPowerUp(int x, int y) : base(x, y)
        {
            
        }
        
        public void Draw(SpriteBatch spriteBatch){
                   spriteBatch.Draw(myTexture, new Rectangle(base.x, base.y, 30, 30),//powerups are 30 by 30
                Color.White);
        }
    }
}
