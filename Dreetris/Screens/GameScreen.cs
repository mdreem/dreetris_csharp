using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    class GameScreen : Screen
    {
        SpriteBatch spriteBatch;
        SpriteFont Font1;

        Texture2D backgroundImage;
        Rectangle backgroundRectangle;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int KEY_PRESSED_TIME = 150;

        TetrisBoard board;
        AssetManager assetManager;
        private Sprite testAnim; //TMP

        public GameScreen(Game game, ScreenManager screenManager, AssetManager assetManager) : base(game, screenManager) 
        {
            this.assetManager = assetManager;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("SpriteFont1");

            // load sprites and build draw rectangles
            backgroundImage = content.Load<Texture2D>("background");
            backgroundRectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            base.LoadContent();
        }

        public override void Initialize()
        {
            ContentManager content = Game.Content;

            board = new TetrisBoard(assetManager, screenManager.keyboard, 10, 20, 80, 100);
            board.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            //animation.Update(gameTime);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw the board and the tetrimino
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, backgroundRectangle, Color.White);
            board.Draw(spriteBatch);
            spriteBatch.DrawString(Font1, "Score: " + board.GetScore().ToString(), new Vector2(500, 20), Color.White);
            spriteBatch.DrawString(Font1, "Level: " + board.level.ToString(), new Vector2(500, 40), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
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
                    PauseScreen ps = new PauseScreen(Game, screenManager);
                    ps.Initialize();
                    screenManager.push(ps);
                }
                if (screenManager.keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    PauseScreen ps = new PauseScreen(Game, screenManager);
                    ps.Initialize();
                    screenManager.push(ps);
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
    }
}
