using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dreetris.Screens
{
    class GameoverScreen : Screen
    {
        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Texture2D blank;

        TetrisBoard board;
        AssetManager assetManager;

        public GameoverScreen(Game game, ScreenManager screenManager, TetrisBoard board, AssetManager assetManager)
            : base(game, screenManager)
        {
            this.board = board;
            this.assetManager = assetManager;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("SpriteFont1");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            base.Update(gameTime);
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Space))
            {
                screenManager.pop();    // pop game over-screen
                screenManager.pop();    // pop game screen

                GameScreen gs = new GameScreen(Game, screenManager, assetManager);
                gs.Initialize();

                TitleScreen ts = new TitleScreen(Game, screenManager, assetManager);
                ts.Initialize();

                screenManager.push(gs);
                screenManager.push(ts);

                screenManager.keyboard.LockKey(Keys.Space);
            }
            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Game Over", new Vector2(300, 120), Color.Red);
            spriteBatch.DrawString(Font1, String.Format("Score: {0}", board.GetScore()), new Vector2(300, 150), Color.Red);
            spriteBatch.DrawString(Font1, "Press Space to Continue", new Vector2(300, 180), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
