using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Burrito
{
    class AnimatedSprite
    {
        //Texture Holder
        private Texture2D t2dTexture;
        //FrameRate Info
        public float fFrameRate = 0.2f;
        public float fElapsed = 0.0f;
        //Offset and size
        private int iFrameOffsetX = 0;
        private int iFrameOffsetY = 0;
        private int iFrameWidth;
        private int iFrameHeight;
        //Checks where our sprite is
        private int iFrameCount = 1;
        private int iCurrentFrame = 0;
        private int iScreenX = 0;
        private int iScreenY = 0;
        //Is our sprite animating?
        private bool bAnimating = true;

        //Constructor
        public AnimatedSprite(Texture2D texture, int FrameOffsetX, int FrameOffsetY,
                                int FrameWidth, int FrameHeight, int FrameCount)
        {
            t2dTexture = texture;
            iFrameOffsetX = FrameOffsetX;
            iFrameOffsetY = FrameOffsetY;
            iFrameWidth = FrameWidth;
            iFrameHeight = FrameHeight;
            iFrameCount = FrameCount;
        }
        
        //Draws our sprite
        public void Draw( SpriteBatch spriteBatch, int XOffset,
                            int YOffset, bool NeedBeginEnd)
        {
            if (NeedBeginEnd)
                spriteBatch.Begin();

            spriteBatch.Draw(
                t2dTexture,
                new Rectangle(
                  iScreenX + XOffset,
                  iScreenY + YOffset,
                  iFrameWidth,
                  iFrameHeight),
                GetSourceRect(),
                Color.White);

            if (NeedBeginEnd)
                spriteBatch.End();
        }
        //Overloaded Draw Function
        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            Draw(spriteBatch, XOffset, YOffset, true);
        }
        
        //Updates our Sprite as time passes
        public void Update(GameTime gametime)
        {
            if (bAnimating)
            {
                // Accumulate elapsed time...
                fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                // Until it passes our frame length
                if (fElapsed > fFrameRate)
                {
                    // Increment the current frame, wrapping back to 0 at iFrameCount
                    iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;

                    // Reset the elapsed frame time.
                    fElapsed = 0.0f;
                }
            }
        }

        //Helper Functions
        public Rectangle GetSourceRect()
        {
            return new Rectangle(
            iFrameOffsetX + (iFrameWidth * iCurrentFrame),
            iFrameOffsetY,
            iFrameWidth,
            iFrameHeight);
        }

        //Getters and Setters
        public int X
        {
            get { return iScreenX; }
            set { iScreenX = value; }
        }
        public int Y
        {
            get { return iScreenY; }
            set { iScreenY = value; }
        }
        public int Frame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount); }
        }
        public float FrameLength
        {
            get { return fFrameRate; }
            set { fFrameRate = (float)Math.Max(value, 0f); }
        }
        public int FrameCount
        {
            get { return iFrameCount; }
        }
        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }
    }
}
