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
        protected Texture2D texture;  //The texture that this obstacle uses
        protected Vector2 position;   //The postion that the obstacle will be drawn (x,y)
        protected Vector2 size;       //The Size of the obstacle (width,height)

        public Obstacle(Texture2D newTexture, Vector2 newPos)
        {
            texture = newTexture;
            position = newPos;
            size = new Vector2(texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            //Don't Draw Obstacles unless they are onscreen
            if((position.X + size.X) > 0 || position.X < Background.screenwidth)
            {
                sb.Draw(texture, position, null, Color.White);
            }
                
        }

        //UPDATE
        //Moves the obstacle to the left deltaX length
        public void Update(float deltaX)
        {
            position.X -= deltaX;
        }
    }
}
