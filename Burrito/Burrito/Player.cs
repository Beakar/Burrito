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
    class Player
    {
        private Texture2D player;
        private Vector2 position;
        private Vector2 velocity;
        //Have we already jumped
        private bool hasJumped;
        //FrameRate info
        private float framesElapsed = 0.0f;
        private float frameRate = 0.1f;
        //Sprite Sheet info (Each point is a position on the Sprite Sheet)
        private Point frameSize = new Point(200, 200);
        private Point currentFrame = new Point(0, 0);
        private Point sheetSize = new Point(5, 4);
        //Sound Info
        SoundEffect[] sound = new SoundEffect[1];
        public Song soundtrack { get; set; }

        public Player(Texture2D newTexture, Vector2 newPosition, SoundEffect[] sounds)
        {
            sound = sounds;
            player = newTexture;
            position = newPosition;
            hasJumped = true;
        }

        public void Update(GameTime gametime)
        {
            position += velocity;

            framesElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (framesElapsed > frameRate)
            {
                framesElapsed -= frameRate;
                //Moves the Source Rectangle

                //Cycle through the sprite sheet
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X - 2)
                {
                    currentFrame.X = 0;
                    //++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }

             if (Keyboard.GetState().IsKeyDown(Keys.Up) && hasJumped == false)
            {
                position.Y -= 20f;  //Initial Jump Speed (Curr: 20f / only change this value)
                velocity.Y = -15f;  //Initial Jump velocity (Curr: -15f / Value must be <0f)
                sound[0].Play();    //Jump Sound Effect
                hasJumped = true;
            }

            if (hasJumped == true)
            {
                float i = 1;
                velocity.Y += 0.5f * i;  //Gravity to be applied (Curr: 0.5f / Increase to add more)
            }

            if (position.Y >= 275)  //Check to make sure player doesnt fall through the floor
                hasJumped = false;

            if (hasJumped == false)
                velocity.Y = 0f;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(player,
                             position,
                             new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                             Color.White);
        }
    }
}
