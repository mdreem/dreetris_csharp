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

        Texture2D background_image;
        Rectangle background_rectangle;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Change resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            animation.add_keyframe(new Keyframe_Straight(new Vector2(600, 200),
                                                new Vector2(650, 250),
                                                1000));

            animation.add_keyframe(new Keyframe_Straight(new Vector2(650, 250),
                                                new Vector2(450, 350),
                                                2000));

            animation.add_keyframe(new Keyframe_Straight(new Vector2(450, 350),
                                                new Vector2(600, 200),
                                                1000));

            bz = new BezierCurve(new Vector2(600, 200), new Vector2(650, 375),
                                             new Vector2(750, 250),
                                             new Vector2(750, 350)
                                              );

            Keyframe_Bezier kb = new Keyframe_Bezier(bz, 2000);

            animation.add_keyframe(kb);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

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
            background_image = Content.Load<Texture2D>("background");
            background_rectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

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
            keyboard.process(gameTime);

            switch (gamestate)
            {
                case State.RUNNING:
                    process_keyboard(gameTime);

                    animation.Update(gameTime);
                    board.Update(gameTime);
                    if (board.gameOver)
                        gamestate = State.GAMEOVER;
                    break;
                case State.TITLE_SCREEN:
                    process_keyboard_title(gameTime);
                    break;
                case State.PAUSED:
                    process_keyboard_paused(gameTime);
                    break;
                case State.GAMEOVER:
                    process_keyboard_game_over(gameTime);
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        private void process_keyboard_title(GameTime gameTime)
        {
            KeyboardState current_state = Keyboard.GetState();

            if (current_state.IsKeyDown(Keys.Space))
            {
                gamestate = State.RUNNING;
                InitializeGame();
                keyboard.lock_key(Keys.Space);
            }
        }

        private void process_keyboard_game_over(GameTime gameTime)
        {
            KeyboardState current_state = Keyboard.GetState();

            if (current_state.IsKeyDown(Keys.Space))
            {
                gamestate = State.TITLE_SCREEN;
                keyboard.lock_key(Keys.Space);
            }
        }

        private void process_keyboard_paused(GameTime gameTime)
        {
            KeyboardState current_state = Keyboard.GetState();

            if (keyboard.is_down(Keys.Enter))
            {
                if (keyboard.changed(Keys.Enter))
                {
                    gamestate = State.RUNNING;
                }
                if (keyboard.is_down_time(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    gamestate = State.RUNNING;
                }
            }

            if (keyboard.is_down(Keys.Escape))
                this.Exit();
        }

        private void process_keyboard(GameTime gameTime)
        {
            if (keyboard.is_down(Keys.Space))
            {
                if (keyboard.changed(Keys.Space))
                {
                    board.flip_tetrimino();
                }
                if (keyboard.is_down_time(Keys.Space) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Space, 3 * KEY_PRESSED_TIME);
                    board.flip_tetrimino();
                }
            }
            //else if so that it is not checked twice. Could flip faster this way
            else if (keyboard.is_down(Keys.Up))
            {
                if (keyboard.changed(Keys.Up))
                {
                    board.flip_tetrimino();
                }
                if (keyboard.is_down_time(Keys.Up) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Up, 3 * KEY_PRESSED_TIME);
                    board.flip_tetrimino();
                }
            }

            if (keyboard.is_down(Keys.Left))
            {
                if (keyboard.changed(Keys.Left))
                {
                    board.move_left();
                }
                if (keyboard.is_down_time(Keys.Left) > KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Left, KEY_PRESSED_TIME);
                    board.move_left();
                }
            }

            if (keyboard.is_down(Keys.Right))
            {
                if (keyboard.changed(Keys.Right))
                {
                    board.move_right();
                }
                if (keyboard.is_down_time(Keys.Right) > KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Right, KEY_PRESSED_TIME);
                    board.move_right();
                }
            }

            if (keyboard.is_down(Keys.Enter))
            {
                if (keyboard.changed(Keys.Enter))
                {
                    gamestate = State.PAUSED;
                }
                if (keyboard.is_down_time(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    keyboard.reset_timer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    gamestate = State.PAUSED;
                }
            }

            if (keyboard.is_down(Keys.Down))
            {
                board.haste();
            }
            if (!keyboard.is_down(Keys.Down))
            {
                board.unhaste();
            }

            if (keyboard.is_down(Keys.Escape))
                this.Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (gamestate)
            {
                case State.RUNNING:
                    Draw_Running(gameTime);
                    break;
                case State.TITLE_SCREEN:
                    Draw_Title(gameTime);
                    break;
                case State.PAUSED:
                    Draw_Paused(gameTime);
                    break;
                case State.GAMEOVER:
                    DrawGameOver(gameTime);
                    break;
                default:
                    break;
            }
            base.Draw(gameTime);
        }

        private void Draw_Paused(GameTime gameTime)
        {
            Draw_Running(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Paused", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Enter to Continue", new Vector2(300, 150), Color.White);

            spriteBatch.End();
        }

        private void DrawGameOver(GameTime gameTime)
        {
            Draw_Running(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Game Over", new Vector2(300, 120), Color.Red);
            spriteBatch.DrawString(Font1, String.Format("Score: {0}", board.get_score()), new Vector2(300, 150), Color.Red);
            spriteBatch.DrawString(Font1, "Press Space to Continue", new Vector2(300, 180), Color.Red);

            spriteBatch.End();
        }

        private void Draw_Title(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.DrawString(Font1, "Dreetris", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Space to Start", new Vector2(300, 150), Color.White);

            spriteBatch.End();
        }

        private void Draw_Running(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw the board and the tetrimino
            spriteBatch.Begin();

            spriteBatch.Draw(background_image, background_rectangle, Color.White);
            board.Draw(spriteBatch);
            spriteBatch.DrawString(Font1, "Score: " + board.get_score().ToString(), new Vector2(500, 20), Color.White);
            spriteBatch.DrawString(Font1, "Level: " + board.level.ToString(), new Vector2(500, 40), Color.White);

            //writer.Write("***current position: ({0}|{1}); Time: {2}; Index: {3}", animation.get_x(), animation.get_y(), animation.current_frame.running_time, animation.index);
            test_rectangle.X = animation.get_x();
            test_rectangle.Y = animation.get_y();

            //spriteBatch.Draw(test_image, test_rectangle, Color.White);

            //draw_long_line(animation.get_path(), Color.White);

            //draw_long_line(bz.subdivide(), Color.White);
            //draw_long_line(bz.get_hull(), Color.Blue);

            spriteBatch.End();
        }

        void draw_long_line(List<Vector2> sublines, Color color)
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
