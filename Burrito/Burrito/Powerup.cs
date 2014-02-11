using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    public abstract class PowerUp: EncounteredObject
    {
        //An Powerup is an EncounteredObject
        //An Powerup doesn't need anything extra of its own *yet*

        public PowerUp(Texture2D newTexture, Vector2 newPos)
            : base(newTexture, newPos)
        {}

        //only draw before it's been hit (make sure to change been hit to false when it is hit)
        public new void Draw(SpriteBatch sb)
        {
                base.Draw(sb);
        }


        public abstract int getPowerUp();//abstract method for all powerups to return their constant


    }
}
