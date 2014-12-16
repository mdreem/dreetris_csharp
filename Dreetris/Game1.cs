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

using Dreetris.Animation;
using Dreetris.Screens;

namespace Dreetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum State
        {
            TITLE_SCREEN,
            RUNNING,
            PAUSED,
            GAMEOVER
        }

        State gamestate = State.TITLE_SCREEN;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D backgroundImage;
        Rectangle backgroundRectangle;

        Texture2D test_image;
        Rectangle test_rectangle;
        Dreetris.Animation.Animation animation = new Dreetris.Animation.Animation();

        Texture2D blank;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int KEY_PRESSED_TIME = 150;

        TetrisBoard board;

        DKeyboard keyboard = new DKeyboard();

        SpriteFont Font1;

        Writer writer = new Writer(500);
        BezierCurve bz;

        ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Change resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            screenManager = new ScreenManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            /*
            animation.addKeyframe(new KeyframeStraight(new Vector2(600, 200),
                                                new Vector2(650, 250),
                                                1000));

            animation.addKeyframe(new KeyframeStraight(new Vector2(650, 250),
                                                new Vector2(450, 350),
                                                2000));

            animation.addKeyframe(new KeyframeStraight(new Vector2(450, 350),
                                                new Vector2(600, 200),
                                                1000));

            bz = new BezierCurve(new Vector2(600, 200), new Vector2(650, 375),
                                             new Vector2(750, 250),
                                             new Vector2(750, 350)
                                              );

            Keyframe_Bezier kb = new Keyframe_Bezier(bz, 2000);

            animation.addKeyframe(kb);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            */

            GameScreen gs = new GameScreen(this, screenManager);
            gs.Initialize();

            TitleScreen ts = new TitleScreen(this, screenManager);
            ts.Initialize();

            screenManager.push(gs);
            screenManager.push(ts);

            base.Initialize();
        }

        void InitializeGame()
        {
            board = new TetrisBoard(Content, keyboard, 10, 20, 80, 100);
            board.CreateTetrimino(Tetrimino.Type.I);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = Content.Load<SpriteFont>("SpriteFont1");
            // load sprites and build draw rectangles
            backgroundImage = Content.Load<Texture2D>("background");
            backgroundRectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            test_image = Content.Load<Texture2D>("block");
            test_rectangle = new Rectangle(600, 200, 20, 20);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            screenManager.Update(gameTime);
            //keyboard.Process(gameTime);

            ////lastKeyboardState = keyboardState;
            ////keyboardState = Keyboard.GetState();

            //switch (gamestate)
            //{
            //    case State.RUNNING:
            //        ProcessKeyboard(gameTime);

            //        animation.Update(gameTime);
            //        board.Update(gameTime);
            //        if (board.gameOver)
            //            gamestate = State.GAMEOVER;
            //        break;
            //    case State.TITLE_SCREEN:
            //        ProcessKeyboardTitle(gameTime);
            //        break;
            //    case State.PAUSED:
            //        ProcessKeyboardPaused(gameTime);
            //        break;
            //    case State.GAMEOVER:
            //        ProcessKeyboardGameOver(gameTime);
            //        break;
            //    default:
            //        break;
            //}
            base.Update(gameTime);
        }

        private void ProcessKeyboardTitle(GameTime gameTime)
        {
            if (keyboard.IsDown(Keys.Space))
            {
                gamestate = State.RUNNING;
                InitializeGame();
                keyboard.LockKey(Keys.Space);
            }
            if (keyboard.IsDown(Keys.Escape))
                this.Exit();
        }

        private void ProcessKeyboardGameOver(GameTime gameTime)
        {
            if (keyboard.IsDown(Keys.Space))
            {
                gamestate = State.TITLE_SCREEN;
                keyboard.LockKey(Keys.Space);
            }
            if (keyboard.IsDown(Keys.Escape))
                this.Exit();
        }

        private void ProcessKeyboardPaused(GameTime gameTime)
        {
            KeyboardState current_state = Keyboard.GetState();

            if (keyboard.IsDown(Keys.Enter))
            {
                if (keyboard.Changed(Keys.Enter))
                {
                    gamestate = State.RUNNING;
                }
                if (keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    gamestate = State.RUNNING;
                }
            }

            if (keyboard.IsDown(Keys.Escape))
                this.Exit();
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (keyboard.IsDown(Keys.Space))
            {
                if (keyboard.Changed(Keys.Space))
                {
                    board.FlipTetrimino();
                }
                if (keyboard.IsDownTime(Keys.Space) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Space, 3 * KEY_PRESSED_TIME);
                    board.FlipTetrimino();
                }
            }
            //else if so that it is not checked twice. Could flip faster this way
            else if (keyboard.IsDown(Keys.Up))
            {
                if (keyboard.Changed(Keys.Up))
                {
                    board.FlipTetrimino();
                }
                if (keyboard.IsDownTime(Keys.Up) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Up, 3 * KEY_PRESSED_TIME);
                    board.FlipTetrimino();
                }
            }

            if (keyboard.IsDown(Keys.Left))
            {
                if (keyboard.Changed(Keys.Left))
                {
                    board.MoveLeft();
                }
                if (keyboard.IsDownTime(Keys.Left) > KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Left, KEY_PRESSED_TIME);
                    board.MoveLeft();
                }
            }

            if (keyboard.IsDown(Keys.Right))
            {
                if (keyboard.Changed(Keys.Right))
                {
                    board.MoveRight();
                }
                if (keyboard.IsDownTime(Keys.Right) > KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Right, KEY_PRESSED_TIME);
                    board.MoveRight();
                }
            }

            if (keyboard.IsDown(Keys.Enter))
            {
                if (keyboard.Changed(Keys.Enter))
                {
                    gamestate = State.PAUSED;
                }
                if (keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    gamestate = State.PAUSED;
                }
            }

            if (keyboard.IsDown(Keys.Down))
            {
                board.Haste();
            }
            if (!keyboard.IsDown(Keys.Down))
            {
                board.Unhaste();
            }

            if (keyboard.IsDown(Keys.Escape))
                this.Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            screenManager.Draw(gameTime);
            //switch (gamestate)
            //{
            //    case State.RUNNING:
            //        DrawRunning(gameTime);
            //        break;
            //    case State.TITLE_SCREEN:
            //        DrawTitle(gameTime);
            //        break;
            //    case State.PAUSED:
            //        DrawPaused(gameTime);
            //        break;
            //    case State.GAMEOVER:
            //        DrawGameOver(gameTime);
            //        break;
            //    default:
            //        break;
            //}
            base.Draw(gameTime);
        }

        private void DrawPaused(GameTime gameTime)
        {
            DrawRunning(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Paused", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Enter to Continue", new Vector2(300, 150), Color.White);

            spriteBatch.End();
        }

        private void DrawGameOver(GameTime gameTime)
        {
            DrawRunning(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Game Over", new Vector2(300, 120), Color.Red);
            spriteBatch.DrawString(Font1, String.Format("Score: {0}", board.GetScore()), new Vector2(300, 150), Color.Red);
            spriteBatch.DrawString(Font1, "Press Space to Continue", new Vector2(300, 180), Color.Red);

            spriteBatch.End();
        }

        private void DrawTitle(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.DrawString(Font1, "Dreetris", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Space to Start", new Vector2(300, 150), Color.White);

            spriteBatch.End();
        }

        private void DrawRunning(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw the board and the tetrimino
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, backgroundRectangle, Color.White);
            board.Draw(spriteBatch);
            spriteBatch.DrawString(Font1, "Score: " + board.GetScore().ToString(), new Vector2(500, 20), Color.White);
            spriteBatch.DrawString(Font1, "Level: " + board.level.ToString(), new Vector2(500, 40), Color.White);

            //writer.Write("***current position: ({0}|{1}); Time: {2}; Index: {3}", animation.get_x(), animation.get_y(), animation.current_frame.running_time, animation.index);
            test_rectangle.X = animation.getX();
            test_rectangle.Y = animation.getY();

            //spriteBatch.Draw(test_image, test_rectangle, Color.White);

            //draw_long_line(animation.get_path(), Color.White);

            //draw_long_line(bz.subdivide(), Color.White);
            //draw_long_line(bz.get_hull(), Color.Blue);

            spriteBatch.End();
        }

        void DrawLongLine(List<Vector2> sublines, Color color)
        {
            for (int i = 0; i < sublines.Count - 1; i++)
            {
                DrawLine(1, color, sublines[i], sublines[i + 1]);
            }
        }

        void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}
