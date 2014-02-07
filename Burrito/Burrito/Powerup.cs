using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    public class PowerUp: EncounteredObject
    {
        //An Powerup is an EncounteredObject
        //An Powerup doesn't need anything extra of its own *yet*

        public bool bBeenHit;
        public PowerUp(Texture2D newTexture, Vector2 newPos)
            : base(newTexture, newPos)
        {}

        //only draw before it's been hit (make sure to change been hit to false when it is hit)
        public override void Draw(SpriteBatch sb){
            if (beenHit == true)
                base.Draw(sb);
        }

        bool beenHit
        {
            get { return bBeenHit; }
            set { beenHit = bBeenHit; }

        }


    }
}
