using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Burrito
{
    public class Obstacle : EncounteredObject
    {
        //An Obstacle is an EncounteredObject
        //An Obstacle doesn't need anything extra of its own *yet*
        public Obstacle(Texture2D newTexture, Vector2 newPos)
            : base(newTexture, newPos)
        {}
    }
}
