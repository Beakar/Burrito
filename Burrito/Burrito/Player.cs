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
        private const int NO_PUP = -1;
        private const int SPEED_PUP = 0;
        private const int JUMP_PUP = 1;
        private const int X_LIFE_PUP = 2;
        public const int SPEED_RESET = -5;
        public int lives = 5;
        public int startTime = 0;
        bool hasSpeedBoost;
        private Texture2D player;
        public Vector2 position;
        public Vector2 velocity;
        public int hasPowerUp = NO_PUP;
        //Have we already jumped
        private bool hasJumped;
        public bool hasJumpPUP;

        private bool _isSliding;

        public int HasPowerUp
        {
            get { return hasPowerUp; }
            set { hasPowerUp = value; }
        }

        public bool IsSliding
        {
            get
            {
                return _isSliding; 
            }
            set
            {
                _isSliding = value;
                if (_isSliding)
                {
                    currentFrame.X = sheetSize.X - 1;
                    if (position.Y <= 290)
                        position.Y += 25;
                }
                else
                {
                    position.Y -= 25;
                }

            }
        }
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
            SetDefaults();
            sound = sounds;
            player = newTexture;
            position = newPosition;
            hasJumped = true;
            
        }

        public void SetDefaults()
        {
            framesElapsed = 0.0f;
            frameRate = 0.1f;
            frameSize = new Point(200, 200);
            currentFrame = new Point(0, 0);
            sheetSize = new Point(5, 4);
            hasPowerUp = NO_PUP;
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

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && IsSliding == false && hasJumped == false)
                sound[1].Play();

            //The player will slide when he presses Keys.Down
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                IsSliding = true;

            //Sliding will end when player lets go of Keys.Down
            if (Keyboard.GetState().IsKeyUp(Keys.Down) && IsSliding)
                IsSliding = false;

            //Uses the Jumping Sprite when you press Keys.Up
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                currentFrame.X = sheetSize.X - 2;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) &&(hasJumpPUP || !hasJumped))
            {
                position.Y -= 20f;  //Initial Jump Speed (Curr: 20f / only change this value)
                velocity.Y = -15f;  //Initial Jump velocity (Curr: -15f / Value must be <0f)
                sound[0].Play();    //Jump Sound Effect
                hasJumped = true;
                if (hasJumpPUP)
                    hasJumpPUP = false;
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

            if (hasPowerUp > -1)
            {
                switch (hasPowerUp)
                {
                    case NO_PUP:
                        break;
                    case SPEED_PUP:
                        if (startTime != 0)
                        {
                            startTime += 4;
                            hasPowerUp = NO_PUP;
                            break;
                        }
                        hasSpeedBoost = true;
                        startTime = (int)gametime.TotalGameTime.TotalSeconds;
                        break;
                    case JUMP_PUP:
                        hasJumpPUP = true;
                        hasPowerUp = NO_PUP;
                        break;
                    case X_LIFE_PUP:
                        //TODO add extra life logic when lives exist
                        break;
                    default:
                        break;
                }
                
            }

            if (hasSpeedBoost && (startTime + 4) < (int)gametime.TotalGameTime.TotalSeconds)
            {
                hasSpeedBoost = false;
                hasPowerUp = SPEED_RESET;
                startTime = 0;
            }

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
