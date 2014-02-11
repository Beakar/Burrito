using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Burrito
{
    public class Hud
    {
        private Vector2 ScorePos = new Vector2(850, 10);
        public Texture2D ScoreBack { get; set; }
        public SpriteFont Font { get; set; }
        public int Score { get; set; }
        public Hud()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScoreBack, new Vector2(0, 0), Color.White);

            spriteBatch.DrawString(Font,
                                   "Score: " + Score.ToString(),
                                   ScorePos,
                                   Color.White);

            spriteBatch.DrawString(Font,
                                   "King Burrito",
                                   new Vector2(435,10),
                                   Color.White);
            
        }
    }
}
