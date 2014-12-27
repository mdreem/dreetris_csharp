using Dreetris.Animation;
using Dreetris.Dreetris;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    class GameScreen : Screen
    {
        #region fields 
        SpriteBatch spriteBatch;
        SpriteFont Font1;

        Texture2D backgroundImage;
        Rectangle backgroundRectangle;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int KEY_PRESSED_TIME = 150;

        TetrisBoard board;
        ScoreBoard score;
        AssetManager assetManager;

        private Texture2D blank;

        #endregion

        public GameScreen(Game game, ScreenManager screenManager, AssetManager assetManager)
            : base(game, screenManager)
        {
            this.assetManager = assetManager;
        }

        #region public methods

        public override void Initialize()
        {
            ContentManager content = Game.Content;

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            board = new TetrisBoard(assetManager, screenManager.keyboard, 10, 20, 80, 100);
            board.Initialize();

            score = new ScoreBoard(board, assetManager);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            board.Update(gameTime);
            if (board.gameOver)
            {
                GameoverScreen gos = new GameoverScreen(Game, screenManager, board, assetManager);
                gos.Initialize();
                screenManager.push(gos);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // draw the board and the tetrimino
            spriteBatch.Draw(backgroundImage, backgroundRectangle, Color.White);
            board.Draw(spriteBatch);
            score.draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region private and protected methods

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("Font");

            // load sprites and build draw rectangles
            backgroundImage = content.Load<Texture2D>("background");
            backgroundRectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            base.LoadContent();
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Space))
            {
                if (screenManager.keyboard.Changed(Keys.Space))
                {
                    board.FlipTetrimino();
                }
                if (screenManager.keyboard.IsDownTime(Keys.Space) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Space, 3 * KEY_PRESSED_TIME);
                    board.FlipTetrimino();
                }
            }
            //else if so that it is not checked twice. Could flip faster this way
            else if (screenManager.keyboard.IsDown(Keys.Up))
            {
                if (screenManager.keyboard.Changed(Keys.Up))
                {
                    board.FlipTetrimino();
                }
                if (screenManager.keyboard.IsDownTime(Keys.Up) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Up, 3 * KEY_PRESSED_TIME);
                    board.FlipTetrimino();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Left))
            {
                if (screenManager.keyboard.Changed(Keys.Left))
                {
                    board.MoveLeft();
                }
                if (screenManager.keyboard.IsDownTime(Keys.Left) > KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Left, KEY_PRESSED_TIME);
                    board.MoveLeft();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Right))
            {
                if (screenManager.keyboard.Changed(Keys.Right))
                {
                    board.MoveRight();
                }
                if (screenManager.keyboard.IsDownTime(Keys.Right) > KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Right, KEY_PRESSED_TIME);
                    board.MoveRight();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Enter))
            {
                if (screenManager.keyboard.Changed(Keys.Enter))
                {
                    pushPauseScreen();
                }
                if (screenManager.keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    pushPauseScreen();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Down))
            {
                board.Haste();
            }
            if (!screenManager.keyboard.IsDown(Keys.Down))
            {
                board.Unhaste();
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }

        private void pushPauseScreen()
        {
            PauseScreen ps = new PauseScreen(Game, screenManager, assetManager);
            ps.Initialize();

            FadeScreen fs = new FadeScreen(Game, screenManager, FadeScreen.Type.FADE_OUT, 0.75f, 250);
            fs.Initialize();

            screenManager.push(ps);
            screenManager.push(fs);
        }

        #endregion
    }
}
