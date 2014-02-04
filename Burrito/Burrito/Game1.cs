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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //BACKGROUND
        Background myBackground;
        //PLAYER
        Player player;
        //Array of Obstacles (Current size: 1)
        Obstacle[] obstacles = new Obstacle[1];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Allows the game to perform any initialization it needs to before starting to run.
        // This is where it can query for any required services and load any non-graphic
        // related content.  Calling base.Initialize will enumerate through any components
        // and initialize them as well.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();
            base.Initialize();
        }

        // LoadContent will be called once per game and is the place to load
        // all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            myBackground = new Background();
            Texture2D background = Content.Load<Texture2D>(@"Textures\Background"); //Load Background

            obstacles[0] = new Obstacle(Content.Load<Texture2D>(@"Textures\angry"), //Load Obstacle
                                     new Vector2(500, 275));                        //At (500,275)

            SoundEffect[] sound = new SoundEffect[1];
            sound[0] = Content.Load<SoundEffect>(@"Sound\cartoon008");  //Load Jump SoundEffect
            player = new Player(Content.Load<Texture2D>(@"Textures\KingBurrito"), new Vector2(100, 275), sound);  //Load Player
            player.soundtrack = Content.Load<Song>(@"Sound\soundtrack2");  //Load Game Soundtrack
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

            // TODO: Add your update logic here
            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            myBackground.Update(elapsed * 300); //Update the background based on time elapsed
            obstacles[0].Update(5);  //Update the obstacles based a set float (keep this float small
            player.Update(gameTime);            //Update player Sprite

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
            player.Draw(spriteBatch);

            //Don't call anything after this line
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
