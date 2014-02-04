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
    public class EncounteredObject
    {
        protected Texture2D texture;  //The texture that this Obstacle uses
        protected Vector2 position;   //The postion that the Obstacle will be drawn (x,y)
        protected Vector2 size;       //The Size of the Obstacle (width,height)
        protected Rectangle hitbox;   //The Obstacle's hitbox

        public EncounteredObject(Texture2D newTexture, Vector2 newPos)
        {
            texture = newTexture;
            position = newPos;
            size = new Vector2(texture.Width, texture.Height);
            //The hitbox is a rectangle 3/4 the size of the texture
            hitbox = new Rectangle((int)(position.X-(size.X *.75)), (int)(position.Y + (size.Y *.25)), (int)(size.X *.9), (int)(size.Y *.85));
        }

        public void Draw(SpriteBatch sb)
        {
            //Don't Draw EncounteredObjects unless they are onscreen
            if((position.X + size.X) > 0 || position.X < Background.screenwidth)
            {
                sb.Draw(texture, position, null, Color.White);
            }
                
        }

        //UPDATE
        //Moves the Object to the left a given (deltaX) length
        public void Update(float deltaX)
        {
            position.X -= deltaX;
            hitbox.X -= (int)deltaX;
        }

        //Checks if the hitbox was touched
        public bool WasHit(int x, int y)
        {
            if (x > hitbox.X && x < (hitbox.X + hitbox.Width))
            {
                if (y < hitbox.Y && y > hitbox.Y - hitbox.Height)
                    return true;
            }
            return false;
        }
    }
}
