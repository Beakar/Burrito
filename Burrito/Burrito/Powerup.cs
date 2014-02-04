using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Burrito
{
    public class PowerUp
    {
        protected Vector2 position = new Vector2(0, 0);
        protected Vector2 size = new Vector2(0, 0);
        protected Rectangle hitBox;

        public PowerUp(Vector2 newPos, Vector2 newSize)
        {
            position = newPos;
            size = newSize;
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public void Draw(SpriteBatch spritebatch);

        public Rectangle getHitbox()
        {
            return hitBox;
        }

    }
}
