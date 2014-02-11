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
        public static int SPEED_PUP = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //BACKGROUND
        Background myBackground;
        int currSpeed = 300;    //The current speed of the game
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
        //SCORE
        int iPlayerScore = 0;
        

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
        // related content.  Calling base.Initialize will enumerate through any components
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
           //powerups

            powerUps.Add(new SpeedPowerUp(speedPUpTex,
                                            new Vector2(2400, 250)));
            powerUps.Add(new SpeedPowerUp(speedPUpTex,
                                            new Vector2(3900, 250)));
            powerUps.Add(new SpeedPowerUp(speedPUpTex,
                                            new Vector2(5400, 250)));

            //Load the obstacles
            LoadObstacles(20, 0);

            SoundEffect[] sound = new SoundEffect[2];
            sound[0] = Content.Load<SoundEffect>(@"Sound\cartoon008");  //Load Jump SoundEffect
            sound[1] = Content.Load<SoundEffect>(@"Sound\cartoon_skid");  //Load slide SoundEffect
            player = new Player(Content.Load<Texture2D>(@"Textures\KingBurrito2"), new Vector2(100, 275), sound);  //Load Player
            player.soundtrack = Content.Load<Song>(@"Sound\soundtrack");  //Load Game Soundtrack
            MediaPlayer.Play(player.soundtrack);  //Play Soundtrack...
            MediaPlayer.IsRepeating = true;       //On Repeat
            myBackground.Load(GraphicsDevice, background);
        }

        // UnloadContent will be called once per game and is the place to unload
        // all content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

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
            updateGameSpeed(gameTime);

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
            //Draw Stuff below this line

            //BACKGROUND DRAWING LOGIC
            myBackground.Draw(spriteBatch);

            //ENCOUNTERED OBJECT DRAWING LOGIC
            foreach (Obstacle x in obstacles)
            {
                if (x.position.X + x.size.X < 0)
                {
                    obstacles.Remove(x);
                    break;
                }
                x.Draw(spriteBatch);
            }

            foreach (PowerUp x in powerUps)
            {
                if (x.position.X + x.size.X < 0)
                {
                    powerUps.Remove(x);
                    break;
                }
                x.Draw(spriteBatch);
            }


            //PLAYER DRAWING LOGIC
            //PowerUp section
            foreach (PowerUp x in powerUps)
            {
                if (x.BeenHit == false && x.WasHit((int)player.position.X, (int)player.position.Y, 200, 200))
                {
                    x.BeenHit = true;
                }
            }

            //Obstacle Section
            foreach (Obstacle x in obstacles)
            {
                if (player.IsSliding && x.WasHit((int)player.position.X + 20, (int)player.position.Y + 100, 160, 80))
                {
                    break;
                }
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
        protected void updateGameSpeed(GameTime gametime)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalMilliseconds;
            timer -= (int)elapsed;

            if (timer <= 0)
            {
                timer = defaultTime;     //Every half second
                currSpeed += speedIncrease;          //Increase currSpeed
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
                    obstacles.Add(new Obstacle(obstacleTex, new Vector2(lastPosition + 1300, 300)));
                //Spawn in Air
                else
                    obstacles.Add(new Obstacle(obstacleTex, new Vector2(lastPosition + 1300, 100)));

                //Increment the spawn location
                lastPosition += 1300;
            }
        }
    }
}
