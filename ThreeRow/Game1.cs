using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;
using ThreeRow.Engine.Systems;
using ThreeRow.ThreeRow;

namespace ThreeRow
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static readonly int SCREEN_WIDTH = 600;
        public static readonly int SCREEN_HEIGHT = 700;
        public static SpriteFont Font;
        public static bool IsGameOver = false;
        public static GameTime GameTime;
        public static bool IsStarted = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Renderer renderer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            GameEngine.Init();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TextureManager.Init(Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderer = new Renderer(spriteBatch);
            GameEngine.GetInstance().SetRenderer(renderer);
            Font = Content.Load<SpriteFont>("GameFont") as SpriteFont;
            SceneManager.GetInstance().LoadScene<MenuScene>();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            if (!IsStarted)
            {
                IsStarted = true;
                GameEngine.GetInstance().Start(gameTime);
            }
            GameEngine.GetInstance().Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);
            renderer.Update(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
