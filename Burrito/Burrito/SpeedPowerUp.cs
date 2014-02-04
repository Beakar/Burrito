using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    public class SpeedPowerUp : PowerUp
    {
        Texture2D myTexture;
<<<<<<< HEAD
        public SpeedPowerUp(int x, int y)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myTexture, new Rectangle(base.position.X, base.position.Y, 30, 30),//powerups are 30 by 30
         Color.White);
=======
        public SpeedPowerUp(int x, int y, Texture2D texture)
            : base(x, y, texture)
        { }
        
        
        public void Draw(SpriteBatch spriteBatch){
            base.Draw(spriteBatch);
>>>>>>> d671f47dac8051fac340d0cbd9257ec471c2f04d
        }
    }
}
