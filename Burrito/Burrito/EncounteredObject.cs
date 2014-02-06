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
            //The hitbox is a rectangle that is smaller than the texture
            hitbox = new Rectangle((int)(position.X-(size.X *.6)), (int)(position.Y + (size.Y *.25)), (int)(size.X *.9), (int)(size.Y *.85));
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
        public bool WasHit(int x, int y, int width, int height)
        {
            //check if player.3 is between 1 & 3
            if (x + width > hitbox.X && x + width < hitbox.X + hitbox.Width)
            { 
                //check if player.4 is between 2 & 4
                if (y + height > hitbox.Y && y + height < hitbox.Y + hitbox.Height)
                {
                    //bottom right corner is in the hitbox
                    return true;
                }
                //check if player.2 is between 2 & 4
                else if (y > hitbox.Y && y < hitbox.Y + hitbox.Height)
                {
                    //top right corner is in the hitbox
                    return true;
                }
                else
                    return false;
            }
            else if (x > hitbox.X && x < hitbox.X + hitbox.Width)
            {
                //check if player.4 is between 2 & 4
                if (y + height > hitbox.Y && y + height < hitbox.Y + hitbox.Height)
                {
                    //bottom left corner is in the hitbox
                    return true;
                }
                //check if player.2 is between 2 & 4
                else if (y > hitbox.Y && y < hitbox.Y + hitbox.Height)
                {
                    //top left corner is in the hitbox
                    return true;
                }
                else
                    return false;
            }
            else
            {/*player.1 & player.3 are not in the hitbox*/}
            return false;
        }
    }
}
