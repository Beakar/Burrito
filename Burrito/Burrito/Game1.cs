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
        //PLAYER
        Player player;
        //TEXTURES
        Texture2D obstacleTex;
        Texture2D speedPUpTex;

        

        int iPlayerScore = 0;
        //Array of Obstacles (Current size: 1)
        List<Obstacle> obstacles = new List<Obstacle>();
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

            myBackground.Update(elapsed * 300); //Update the background based on time elapsed
            
            //Obstacle Updating
            foreach (Obstacle x in obstacles)
            {
                x.Update((float)8);
            }

            //PowerUP updating
            foreach (PowerUp x in powerUps)
            {
                x.Update((float)7);
            }
            player.Update(gameTime);            //Update player's sprite

            base.Update(gameTime);
        }

        // This is called when the game should draw itself.
        // Parameter<gameTime>: Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //Draw Stuff below this line

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
            foreach (PowerUp x in powerUps)
            {
                if (x.BeenHit == false && x.WasHit((int)player.position.X, (int)player.position.Y, 200, 200))
                {
                    x.BeenHit = true;
                }
            }
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
