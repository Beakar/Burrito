using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Burrito
{
    class GamePlayScreen
    {
        private Game1 game;

        public static int DEFAULT_SPEED = 475;
        public static int SPEED_RESET = -5;
        public static int NO_PUP = -1;
        public static int SPEED_PUP = 0;
        public static int JUMP_PUP = 1;
        public static int EXTRA_LIFE_PUP = 2;
        public static int MAX_LIVES = 5;
        public static int DEFAULT_TIME = 400;  //Increase speed every 500ms
        public static int POINTS_UNTIL_FATTENING = 600;  //how many points until your player gets increased weight

        public bool hasSpeedBoost;

        HUD hud;
        int scorePerSecond = 1;
        int scoreGained = 0;

        AnimatedSprite explosion;
        Random generator = new Random();
        //BACKGROUND
        Background myBackground;
        //SPEED STUFF
        int currSpeed = DEFAULT_SPEED;    //The current speed of the game
        int speedIncrease = 1;  //How fast the game speeds up
        int timesSpedUp = 0;
        //PLAYER
        Player player;
        bool lostLife = false;
        bool canLoseLife = true;
        int gracePeriod = 300;
        //TEXTURES
        Texture2D lowObstacleTex;
        Texture2D highObstacleTex;
        Texture2D speedPUpTex;
        Texture2D extraLifePUpTex;
        Texture2D jumpPUpTex;
        Texture2D explosionTex;
        //TIMER
        int timer = DEFAULT_TIME;        //Starting timer
        //OBSTACLES
        List<Obstacle> obstacles = new List<Obstacle>();
        //POWER-UPS
        List<PowerUp> powerUps = new List<PowerUp>();
        //COLLECTIBLES
        List<Collectible> collectibles = new List<Collectible>();
        List<Texture2D> collectibleTextures = new List<Texture2D>();

        public GamePlayScreen(Game1 game)
        {
            this.game = game;

            lowObstacleTex = game.Content.Load<Texture2D>(@"Textures\lowobstacle");
            highObstacleTex = game.Content.Load<Texture2D>(@"Textures\highobstacle");

            speedPUpTex = game.Content.Load<Texture2D>(@"Textures\speedpup");
            extraLifePUpTex = game.Content.Load<Texture2D>(@"Textures\1uppup");
            jumpPUpTex = game.Content.Load<Texture2D>(@"Textures\jumppup");
            explosionTex = game.Content.Load<Texture2D>(@"Textures\explosions");

            Texture2D lettuceCollect = game.Content.Load<Texture2D>(@"Textures\lettucecollect");
            collectibleTextures.Add(lettuceCollect);
            Texture2D riceCollect = game.Content.Load<Texture2D>(@"Textures\ricecollect");
            collectibleTextures.Add(riceCollect);
            Texture2D tomatoCollect = game.Content.Load<Texture2D>(@"Textures\tomatocollect");
            collectibleTextures.Add(tomatoCollect);

            explosion = new AnimatedSprite(game.Content.Load<Texture2D>(@"Textures\Explosions"),
                                           0, 0, 64, 64, 16);
            explosion.X = 0;
            explosion.Y = 0;


            // TODO: use this.Content to load your game content here
            myBackground = new Background();
            Texture2D background = game.Content.Load<Texture2D>(@"Textures\bg1"); //Load Background

            SoundEffect[] sound = new SoundEffect[2];
            sound[0] = game.Content.Load<SoundEffect>(@"Sound\cartoon008");  //Load Jump SoundEffect
            sound[1] = game.Content.Load<SoundEffect>(@"Sound\cartoon_skid");  //Load slide SoundEffect

            player = new Player(game.Content.Load<Texture2D>(@"Textures\KingBurrito"), new Vector2(100, 275), sound);  //Load Player

            hud = new HUD(player.lives);
            hud.Font = game.Content.Load<SpriteFont>(@"Fonts\Pericles");
            hud.Back = game.Content.Load<Texture2D>(@"Textures\scoreback");

            //Load the PowerUps
            LoadEncounteredObjects(8, 0);

            //Load the obstacles

            LoadObstacles(8, 0);
            player.soundtrack = game.Content.Load<Song>(@"Sound\soundtrack");  //Load Game Soundtrack

            MediaPlayer.Play(player.soundtrack);  //Play Soundtrack...
            MediaPlayer.IsRepeating = true;       //On Repeat
            myBackground.Load(game.GraphicsDevice, background);
        }

        //UPDATE//
        public void Update(GameTime gameTime)
        {
            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateGameSpeed(gameTime);

            //Obstacle Updating
            foreach (Obstacle x in obstacles)
            {
                x.Update(elapsed * currSpeed);
                //SLIDING
                if (player.IsSliding && x.WasHit((int)player.position.X + 20, (int)player.position.Y + 100, 160, 80) && canLoseLife)
                {
                    lostLife = true;
                }
                //NO SLIDING
                else if ((!player.IsSliding && x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160)) && canLoseLife)
                {
                    lostLife = true;
                }
            }

            Death(gameTime);

            myBackground.Update(elapsed * currSpeed); //Update the background based on time elapsed

            //PowerUP updating
            foreach (PowerUp x in powerUps)
            {
                x.Update(elapsed * currSpeed);

            }

            if (player.HasPowerUp == SPEED_PUP && !hasSpeedBoost)
            {
                currSpeed += 200;
                hasSpeedBoost = true;
                player.HasPowerUp = NO_PUP;
            }
            if (player.HasPowerUp == SPEED_RESET)
            {
                currSpeed = DEFAULT_SPEED;
                hasSpeedBoost = false;
                player.HasPowerUp = NO_PUP;
            }

            //Update player's sprite
            player.Update(gameTime);
        }

        //DRAW//
        public void Draw(SpriteBatch spriteBatch)
        {
            //BACKGROUND DRAWING LOGIC//
            myBackground.Draw(spriteBatch);

            if ((hud.Score - scoreGained) > POINTS_UNTIL_FATTENING)
            {
                player.MakeFatter();
                scoreGained = hud.Score;
            }

            //ENCOUNTERED OBJECT LOGIC//
            //Remove obstacles that are offscreen
            foreach (Obstacle x in obstacles)
            {
                if (x.position.X + x.size.X < 0)
                {
                    obstacles.Remove(x);
                    break;
                }
            }
            //Draw my obstacles
            foreach (Obstacle x in obstacles)
                x.Draw(spriteBatch);


            //Load (6) more obstacles if the amount is running low
            if (obstacles.Count < 5)
            {
                LoadObstacles(6, (int)(obstacles[obstacles.Count - 1].position.X));
            }

            //POWERUP DRAWING LOGIC//
            foreach (PowerUp x in powerUps)
            {
                if (x.position.X + x.size.X < 0)
                {
                    powerUps.Remove(x);
                    break;
                }
            }
            foreach (PowerUp x in powerUps)
                x.Draw(spriteBatch);

            foreach (Collectible x in collectibles)
                x.Draw(spriteBatch);

            //Load (6) more obstacles if the amount is running low
            if (powerUps.Count < 5)
            {
                LoadEncounteredObjects(3, (int)(powerUps[powerUps.Count - 1].position.X));
            }

            //PLAYER DRAWING LOGIC//
            //PowerUp section
            foreach (PowerUp x in powerUps)
            {
                if (x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160))
                {
                    player.HasPowerUp = x.getPowerUp();

                    if (x.getPowerUp() == EXTRA_LIFE_PUP && player.lives < MAX_LIVES)
                        hud.Lives = player.lives;

                    powerUps.Remove(x);
                    hud.Score += 20;
                    break;
                }
            }

            //Obstacle Section
            foreach (Obstacle x in obstacles)
            {
                //SLIDING
                if (player.IsSliding && x.WasHit((int)player.position.X + 20, (int)player.position.Y + 100, 160, 80))
                {
                    drawExplosion();
                    break;
                }
                //NOT SLIDING
                else if (!player.IsSliding && x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160))
                {
                    drawExplosion();
                    break;
                }
                else
                    player.Draw(spriteBatch);
            }
            hud.Draw(spriteBatch);
        }

        protected void Death(GameTime gameTime)
        {
            if (lostLife && canLoseLife)
            {
                player.lives--;
                hud.Lives = player.lives;
            }
            
            if (player.lives == 0)
                game.EndGame();

            canLoseLife = false;
            float deathElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            gracePeriod -= (int)deathElapsed;
            if (gracePeriod <= 0)
            {
                lostLife = false;
                canLoseLife = true;
                gracePeriod = 300;
            }
        }

        //Makes the game move faster over time
        protected void UpdateGameSpeed(GameTime gametime)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalMilliseconds;
            timer -= (int)elapsed;
            if (timer <= 0)
            {
                hud.Score += scorePerSecond;
                timer = DEFAULT_TIME;         //Every DEFAULT_TIME
                currSpeed += speedIncrease;  //Increase currSpeed
                ++timesSpedUp;
            }
            if (timesSpedUp > 20)
            {
                scorePerSecond += 1;
                timesSpedUp = 0;
            }
        }

        //Loads a given number of Obstacles
        protected void LoadObstacles(int numObstacles, int lastPosition)
        {
            Random generator = new Random();
            for (int i = 0; i < numObstacles; ++i)
            {
                int rand = generator.Next(0, 50);
                //Spawn on Floor
                if (rand < 30)
                    obstacles.Add(new Obstacle(lowObstacleTex, new Vector2(lastPosition + 1000, 350)));
                //Spawn in Air
                else
                    obstacles.Add(new Obstacle(highObstacleTex, new Vector2(lastPosition + 1000, -50)));

                //Increment the spawn location
                lastPosition += 1000;
            }
        }


        //Loads a given number of Power Ups
        protected void LoadEncounteredObjects(int numEncounteredObjects, int lastPosition)
        {
            Random generator = new Random();
            for (int i = 0; i < numEncounteredObjects; i++)
            {
                int rand = generator.Next(0, 100);

                if (rand < 25)
                {
                    if (rand < 5)
                    {
                        powerUps.Add(new ExtraLifePowerUp(extraLifePUpTex,
                                                          new Vector2(lastPosition + (100 * generator.Next(20, 30)), 10 * generator.Next(7, 30))));
                    }
                    else if (rand < 15)
                    {
                        powerUps.Add(new SpeedPowerUp(speedPUpTex,
                                           new Vector2(lastPosition + (100 * generator.Next(20, 30)), 10 * generator.Next(7, 30))));
                    }

                    else if (rand < 25)
                    {
                        powerUps.Add(new JumpPowerUp(jumpPUpTex,
                                           new Vector2(lastPosition + (100 * generator.Next(20, 30)), 10 * generator.Next(7, 30))));
                    }

                }
                else
                {
                    rand = generator.Next(0, 2);

                    collectibles.Add(new Collectible(collectibleTextures[rand],
                                     new Vector2(lastPosition + (100 * generator.Next(20, 30)), 10 * generator.Next(7, 30)), 20));

                }
                lastPosition += 1700;
            }
        }

        public void drawExplosion()
        {
            int k = -10;
            for (int i = -10; i <= 140; i += 20)
            {
                explosion.Draw(game.spriteBatch, (int)player.position.X + k + 75, (int)player.position.Y + i, false);

                if (k == -10)
                    k = 20;
                else
                    k = -10;
            }
        }

        public int NumLives()
        {
            return player.lives;
        }
    }
}
