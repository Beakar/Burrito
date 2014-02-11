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
    class Collectable : EncounteredObject
    {
        private int _Worth;
        public int Worth
        {
            get { return _Worth; }
            set { _Worth = value; }
        }

        public Collectable(Texture2D newTexture, Vector2 newPos, int score)
            : base(newTexture, newPos)
        {
            _Worth = score;
        }
    }
}
