using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Burrito
{
    // This is the main type for your game
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static int DEFAULT_SPEED = 300;
        public static int SPEED_RESET = -5;
        public static int NO_PUP = -1;
        public static int SPEED_PUP = 0;
        public static int JUMP_PUP = 1;
        public static int EXTRA_LIFE_PUP = 2;

        public static int MAX_LIVES = 3;
        int lives;

        public bool hasSpeedBoost;

        HUD hud;

        AnimatedSprite explosion;
        Random generator = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //BACKGROUND
        Background myBackground;

        int currSpeed = DEFAULT_SPEED;    //The current speed of the game
        int speedIncrease = 2;  //How fast the game speeds up
        //PLAYER
        Player player;
        //TEXTURES
        Texture2D lowObstacleTex;
        Texture2D highObstacleTex;

        Texture2D speedPUpTex;
        Texture2D extraLifePUpTex;
        Texture2D jumpPUpTex;
        Texture2D explosionTex;

        //TIMER
        int defaultTime = 400;  //Increase speed every 400ms
        int timer = 400;        //Starting timer
        //OBSTACLES
        List<Obstacle> obstacles = new List<Obstacle>();
        //POWER-UPS
        List<PowerUp> powerUps = new List<PowerUp>();

        //COLLECTIBLES
        List<Collectible> collectibles = new List<Collectible>();

        List<Texture2D> collectibleTextures = new List<Texture2D>();



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void StartNewGame()
        {

        }

        // Allows the game to perform any initialization it needs to before starting to run.
        // This is where it can query for any required services and load any non-graphic
        // related content. Calling base.Initialize will enumerate through any components
        // and initialize them as well.
        protected override void Initialize()
        {
            //Creates our preferred screen size
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();
            base.Initialize();
        }

        // LoadContent will be called once per game and is the place to load
        // all of your content.
        protected override void LoadContent()
        {
            lives = 5;
            lowObstacleTex = Content.Load<Texture2D>(@"Textures\lowobstacle");
            highObstacleTex = Content.Load<Texture2D>(@"Textures\highobstacle");

            speedPUpTex = Content.Load<Texture2D>(@"Textures\speedpup");
            extraLifePUpTex = Content.Load<Texture2D>(@"Textures\1uppup");
            jumpPUpTex = Content.Load<Texture2D>(@"Textures\jumppup");
            explosionTex = Content.Load<Texture2D>(@"Textures\explosions");

            Texture2D lettuceCollect = Content.Load<Texture2D>(@"Textures\lettucecollect");
            collectibleTextures.Add(lettuceCollect);
            Texture2D riceCollect = Content.Load<Texture2D>(@"Textures\ricecollect");
            collectibleTextures.Add(riceCollect);
            Texture2D tomatoCollect = Content.Load<Texture2D>(@"Textures\tomatocollect");
            collectibleTextures.Add(tomatoCollect);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            explosion = new AnimatedSprite(Content.Load<Texture2D>(@"Textures\Explosions"),
                                           0, 0, 64, 64, 16);
            explosion.X = 0;
            explosion.Y = 0;


            // TODO: use this.Content to load your game content here
            myBackground = new Background();
            Texture2D background = Content.Load<Texture2D>(@"Textures\bg1"); //Load Background

            hud = new HUD(lives);
            hud.Font = Content.Load<SpriteFont>(@"Fonts\Pericles");
            hud.Back = Content.Load<Texture2D>(@"Textures\scoreback");

            //Load the PowerUps
            LoadEncounteredObjects(8, 0);

            //Load the obstacles
            LoadObstacles(8, 0);

            SoundEffect[] sound = new SoundEffect[2];
            sound[0] = Content.Load<SoundEffect>(@"Sound\cartoon008");  //Load Jump SoundEffect
            sound[1] = Content.Load<SoundEffect>(@"Sound\cartoon_skid");  //Load slide SoundEffect

            player = new Player(Content.Load<Texture2D>(@"Textures\KingBurrito"), new Vector2(100, 275), sound);  //Load Player
            player.soundtrack = Content.Load<Song>(@"Sound\soundtrack");  //Load Game Soundtrack

            MediaPlayer.Play(player.soundtrack);  //Play Soundtrack...
            MediaPlayer.IsRepeating = true;       //On Repeat
            myBackground.Load(GraphicsDevice, background);
        }

        // UnloadContent will be called once per game and is the place to unload
        // all content.
        protected override void UnloadContent()
        { }

        // Allows the game to run logic such as updating the world,
        // checking for collisions, gathering input, and playing audio.
        // Parameter<gameTime>: Provides a snapshot of timing values.
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Background Updating based on Time Elapsed (TO DO: Increase Speed over time)
            UpdateGameSpeed(gameTime);

            //Obstacle Updating
            foreach (Obstacle x in obstacles)
            {
                x.Update(elapsed * currSpeed);
            }

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
                currSpeed -= 200;
                hasSpeedBoost = false;
                player.HasPowerUp = NO_PUP;
            }




            //Update player's sprite
            player.Update(gameTime);

            base.Update(gameTime);
        }

        // This is called when the game should draw itself.
        // Parameter<gameTime>: Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //BACKGROUND DRAWING LOGIC//
            myBackground.Draw(spriteBatch);

            hud.Draw(spriteBatch);

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

                    if (x.getPowerUp() == EXTRA_LIFE_PUP && lives < MAX_LIVES)
                        lives++;

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
                    if (lives > 0)
                    {
                        lives--;
                        hud.Lives = lives;
                    }
                    drawExplosion();
                    break;
                }
                //NOT SLIDING
                else if (!player.IsSliding && x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160))
                {
                    if (lives > 0)
                    {
                        lives--;
                        hud.Lives = lives;
                    }
                    drawExplosion();
                    break;
                }
                else
                    player.Draw(spriteBatch);

            }
            if (obstacles.Count == 0)
            {
                player.Draw(spriteBatch);
            }

            //Don't call anything after this line
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Makes the game move faster over time
        protected void UpdateGameSpeed(GameTime gametime)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalMilliseconds;
            timer -= (int)elapsed;
            if (timer <= 0)
            {
                hud.Score += 2;
                timer = defaultTime;         //Every defaultTime
                currSpeed += speedIncrease;  //Increase currSpeed
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
                //else if (rand < 40)
                //{                
                //    //TODO Textures for new powerups
                //        powerUps.Add(new JumpPowerUp(speedPUpTex,
                //                         new Vector2(lastPosition + (100*generator.Next(20, 30)), 10 * generator.Next(7, 30))));
                //    }
                //    else
                //    {
                //        //TODO Textures for new powerups
                //        powerUps.Add(new ExtraLifePowerUp(speedPUpTex,
                //                         new Vector2(lastPosition + (100*generator.Next(20, 30)), 10 * generator.Next(7, 30))));
            }

            lastPosition += 2000;
        }

        public void drawExplosion()
        {
            int k = -10;
            for (int i = -10; i <= 140; i += 20)
            {
                explosion.Draw(spriteBatch, (int)player.position.X + k + 75, (int)player.position.Y + i, false);

                if (k == -10)
                    k = 20;
                else
                    k = -10;
            }
        }

    }

}

