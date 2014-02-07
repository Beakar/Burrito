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
        Obstacle[] obstacles = new Obstacle[3];
        PowerUp[] powerUps = new PowerUp[3];

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

            powerUps[0] = new SpeedPowerUp(speedPUpTex,
                                            new Vector2(2400, 250));
            powerUps[1] = new SpeedPowerUp(speedPUpTex,
                                            new Vector2(3400, 250));
            powerUps[2] = new SpeedPowerUp(speedPUpTex,
                                            new Vector2(4400, 250));


            obstacles[0] = new Obstacle(obstacleTex, //Load Obstacle
                                     new Vector2(2000, 250));                        //At (1000,275)
            obstacles[1] = new Obstacle(obstacleTex, //Load Obstacle
                                     new Vector2(3000, 150));                        //At (1000,275)
            obstacles[2] = new Obstacle(obstacleTex, //Load Obstacle
                                     new Vector2(4000, 250));                        //At (1000,275)

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

            // TODO: Add your game logic here.
            myBackground.Update(elapsed * 300); //Update the background based on time elapsed
            obstacles[0].Update((float)7.5);    //Update the obstacles based a set float (keep this float small)
            obstacles[1].Update((float)7.5);    //Upd based a set float (keep this float small)ate the obstacles based a set float (keep this float small)
            obstacles[2].Update((float)7.5);    //Update the obstacles
            powerUps[0].Update((float)7.5);    
            powerUps[1].Update((float)7.5);    
            powerUps[2].Update((float)7.5);
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
            obstacles[0].Draw(spriteBatch);
            obstacles[1].Draw(spriteBatch);
            obstacles[2].Draw(spriteBatch);
            powerUps[0].Draw(spriteBatch);
            powerUps[1].Draw(spriteBatch);
            powerUps[2].Draw(spriteBatch);
            foreach (PowerUp x in powerUps)
            {
                if (x.BeenHit == false && x.WasHit((int)player.position.X, (int)player.position.Y, 200, 200))
                {
                    x.BeenHit = true;
                }
            }
            foreach (Obstacle x in obstacles)
            {
                if (x.WasHit((int)player.position.X, (int)player.position.Y, 200, 200))
                {
                    break;
                }
                else
                    player.Draw(spriteBatch);
                //else
                //{
                //    this.Exit();
                //    player.SetDefaults();
                //}
            }
            
            //Don't call anything after this line
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
