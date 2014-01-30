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
    class Obstacle
    {
        protected Texture2D obstacle;
        protected Vector2 position = new Vector2(0,0);
        protected Vector2 size = new Vector2(100,100);

        public Obstacle(Texture2D newTexture, Vector2 newPos, Vector2 newSize)
        {
            obstacle = newTexture;
            position = newPos;
            size = newSize;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(obstacle, position, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
        }
    }
}
