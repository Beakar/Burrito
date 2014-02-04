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
        public SpeedPowerUp(int x, int y, Texture2D texture)
            : base(x, y, texture)
        { }
        
        
        public void Draw(SpriteBatch spriteBatch){
            base.Draw(spriteBatch);
        }
    }
}
