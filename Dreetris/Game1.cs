using Dreetris.Animation;
using Dreetris.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Dreetris
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        ScreenManager screenManager;
        AssetManager assetManager;
        private Texture2D blank;
        private GameObjects gameObjects;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }

        protected override void Initialize()
        {
            SoundEffect.MasterVolume = 0.025f;

            assetManager = new AssetManager(this.Content);
            screenManager = new ScreenManager(this);

            gameObjects = new GameObjects(this, screenManager, assetManager);

            GameScreen gs = new GameScreen(gameObjects);
            gs.Initialize();

            FadeScreen fs = new FadeScreen(gameObjects, FadeScreen.Type.FADE_IN);
            fs.Initialize();

            TitleScreen ts = new TitleScreen(gameObjects);
            ts.Initialize();

            screenManager.Push(gs);
            screenManager.Push(fs);
            screenManager.Push(ts);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Drawing.Initialize(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            Song song = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            screenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
