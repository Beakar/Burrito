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
        public PowerUp(Texture2D newTexture, Vector2 newPos)
            : base(newTexture, newPos)
        {}

    }
}
