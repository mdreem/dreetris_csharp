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
        GameObjects gameObjects;

        private Texture2D blank;

        #endregion

        public GameScreen(GameObjects gameObjects)
            : base(gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        #region public methods

        public override void Initialize()
        {
            ContentManager content = Game.Content;

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            board = new TetrisBoard(gameObjects.assetManager, screenManager.keyboard, 10, 20, 80, 100);
            board.Initialize();

            score = new ScoreBoard(board, gameObjects.assetManager);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            board.Update(gameTime);
            if (board.gameOver)
            {
                GameoverScreen gos = new GameoverScreen(gameObjects, board);
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
            if (screenManager.keyboard.downRepeated(Keys.Space, 3 * KEY_PRESSED_TIME))
            {
                board.FlipTetrimino();
            }
            //else if so that it is not checked twice. Could flip faster this way
            else if (screenManager.keyboard.downRepeated(Keys.Up, 3 * KEY_PRESSED_TIME))
            {
                board.FlipTetrimino();
            }

            if (screenManager.keyboard.downRepeated(Keys.Left, KEY_PRESSED_TIME))
            {
                board.MoveLeft();
            }

            if (screenManager.keyboard.downRepeated(Keys.Right, KEY_PRESSED_TIME))
            {
                board.MoveRight();
            }

            if (screenManager.keyboard.downRepeated(Keys.Enter, 3 * KEY_PRESSED_TIME))
            {
                pushPauseScreen();
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
            PauseScreen ps = new PauseScreen(gameObjects);
            ps.Initialize();

            FadeScreen fs = new FadeScreen(gameObjects, FadeScreen.Type.FADE_OUT, 0.75f, 250);
            fs.Initialize();

            screenManager.push(ps);
            screenManager.push(fs);
        }

        #endregion
    }
}
