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
        public static int DEFAULT_SPEED = 400;
        public static int SPEED_RESET = -5;
        public static int NO_PUP = -1;
        public static int SPEED_PUP = 0;
        public static int JUMP_PUP = 1;
        public static int EXTRA_LIFE_PUP = 2;
        public bool hasSpeedBoost;

        HUD hud;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //BACKGROUND
        Background myBackground;
        int currSpeed = DEFAULT_SPEED;    //The current speed of the game
        int speedIncrease = 2;  //How fast the game speeds up
        //PLAYER
        Player player;
        //TEXTURES
        Texture2D obstacleTex;
        Texture2D speedPUpTex;
        //TIMER
        int defaultTime = 400;  //Increase speed every 400ms
        int timer = 400;        //Starting timer
        //OBSTACLES
        List<Obstacle> obstacles = new List<Obstacle>();
        //POWER-UPS
        List<PowerUp> powerUps = new List<PowerUp>();
        

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

            obstacleTex = Content.Load<Texture2D>(@"Textures\angry");
            speedPUpTex = Content.Load<Texture2D>(@"Textures\jalapeno");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            myBackground = new Background();
            Texture2D background = Content.Load<Texture2D>(@"Textures\Background"); //Load Background

            hud = new HUD();
            hud.Font = Content.Load<SpriteFont>(@"Fonts\Pericles");
            hud.Back = Content.Load <Texture2D>(@"Textures\scoreback");

            //Load the PowerUps
            LoadPowerups(8, 0);

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
        {}

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

             if(player.HasPowerUp == SPEED_PUP && !hasSpeedBoost)
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
                LoadObstacles(6, (int)(obstacles[obstacles.Count-1].position.X));
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

            //Load (3) more powerups if the amount is running low
            if (powerUps.Count < 5)
            {
                LoadPowerups(6, (int)(powerUps[powerUps.Count - 1].position.X));
            }

            //PLAYER DRAWING LOGIC//
            //PowerUp section
            foreach (PowerUp x in powerUps)
            {
                if (x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160))
                {
                    player.HasPowerUp = x.getPowerUp();
                    powerUps.Remove(x);
                    break;
                }
            }

            //Obstacle Section
            foreach (Obstacle x in obstacles)
            {
                //SLIDING
                if (player.IsSliding && x.WasHit((int)player.position.X + 20, (int)player.position.Y + 100, 160, 80))
                {
                    break;
                }
                //NOT SLIDING
                else if (!player.IsSliding && x.WasHit((int)player.position.X + 80, (int)player.position.Y + 20, 100, 160))
                {
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
                    obstacles.Add(new Obstacle(obstacleTex, new Vector2(lastPosition + 1000, 300)));
                //Spawn in Air
                else
                    obstacles.Add(new Obstacle(obstacleTex, new Vector2(lastPosition + 1000, 100)));

                //Increment the spawn location
                lastPosition += 1000;
            }
        }


        //Loads a given number of Power Ups
        protected void LoadPowerups(int numPowerUps, int lastPosition)
        {
            Random generator = new Random();
            for (int i = 0; i < numPowerUps; ++i)
            {
                int rand = generator.Next(0, 60);
                if (rand < 20)
                {
                    powerUps.Add(new SpeedPowerUp(speedPUpTex,
                                       new Vector2(lastPosition + (100*generator.Next(20, 30)), 10 * generator.Next(7, 30))));
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
       }
    }

