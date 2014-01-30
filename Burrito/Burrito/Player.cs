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
        private bool hasJumped;

        //FrameRate info
        private float fElapsed = 0.0f;
        private float frameRate = 0.05f;
        //Sprite Sheet info (Each point is a position on the Sprite Sheet)
        private Point frameSize = new Point(200, 200);
        private Point currentFrame = new Point(0, 0);
        private Point sheetSize = new Point(5, 4);

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

            fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (fElapsed > frameRate)
            {
                fElapsed -= frameRate;
                //Moves the Source Rectangle

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
<<<<<<< HEAD
<<<<<<< HEAD
                position.Y -= 20f;
                velocity.Y = -15f;
=======
                position.Y -= 10f;  //Initial Jump Speed (frames)
                velocity.Y = -10f;  //Velocity up (frames)
>>>>>>> 8a86ce2df8991c6cce90ca539679101bbe4cc9cc
=======
                position.Y -= 10f;  //Initial Jump Speed (frames)
                velocity.Y = -10f;  //Velocity up (frames)
>>>>>>> 8a86ce2df8991c6cce90ca539679101bbe4cc9cc
                sound[0].Play();
                hasJumped = true;
            }

            if (hasJumped == true)
            {
                float i = 1;
<<<<<<< HEAD
<<<<<<< HEAD
                velocity.Y += 0.7f * i;
=======
                velocity.Y += 0.5f * i;  //Applys gravity, increase to fall faster (Current Value: 0.5f)
>>>>>>> 8a86ce2df8991c6cce90ca539679101bbe4cc9cc
=======
                velocity.Y += 0.5f * i;  //Applys gravity, increase to fall faster (Current Value: 0.5f)
>>>>>>> 8a86ce2df8991c6cce90ca539679101bbe4cc9cc
            }

            if (position.Y >= 200)
                hasJumped = false;

            if (hasJumped == false)
                velocity.Y = 0f;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(player, position, new Rectangle(currentFrame.X * frameSize.X, 
                currentFrame.Y * frameSize.Y,
                frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }
}
