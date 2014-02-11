using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Burrito
{
    public class HUD
    {
        private Vector2 scorePos = new Vector2(850, 10);

        public SpriteFont Font { get; set; }

        public Texture2D Back { get; set; }

        public int Score { get; set; }

        public HUD()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Back,
                             new Vector2(0, 0),
                             Color.White);

            // Draw the Score in the top-left of screen
            spriteBatch.DrawString(
                Font,                          // SpriteFont
                "Score: " + Score.ToString(),  // Text
                scorePos,                      // Position
                Color.White);                  // Tint

            spriteBatch.DrawString(
                Font,
                "King Burrito",
                new Vector2(450, 10),
                Color.White);
        }
    }
}
