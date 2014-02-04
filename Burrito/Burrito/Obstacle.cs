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
        protected Vector2 position;
        protected Vector2 size;

        public Obstacle(Texture2D newTexture, Vector2 newPos)
        {
            obstacle = newTexture;
            position = newPos;
            size = new Vector2(obstacle.Width, obstacle.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(obstacle, position, 
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                Color.White);
        }

        public void Update(float deltaX)
        {
            position.X -= deltaX;
            position.X = position.X % obstacle.Width;
        }
    }
}
