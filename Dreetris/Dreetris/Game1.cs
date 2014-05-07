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

namespace Dreetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int KEY_PRESSED_TIME = 150;

        TetrisBoard board;

        KeyboardState last_state;
        KeyboardState current_state;

        double time_since_last_step = 0;

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
            board = new TetrisBoard(Content, 10, 25, 20, 20);
            board.CreateTetrimino(Tetrimino.Type.I);
            
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

            // load sprites and build draw rectangles
            // test_sprite = Content.Load<Texture2D>("KopfDings");
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
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState current_state = Keyboard.GetState();

            time_since_last_step += time;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (current_state.IsKeyDown(Keys.Escape))
                this.Exit();

            if (is_key_pressed(Keys.Space, 3*KEY_PRESSED_TIME))
            {
                board.flip_tetrimino();
                time_since_last_step = 0;
            }

            if (is_key_pressed(Keys.Up, 3*KEY_PRESSED_TIME))
            {
                board.flip_tetrimino();
                time_since_last_step = 0;
            }

            if (is_key_pressed(Keys.Left, KEY_PRESSED_TIME))
            {
                board.move_left();
                time_since_last_step = 0;
            }

            if (is_key_pressed(Keys.Right, KEY_PRESSED_TIME))
            {
                board.move_right();
                time_since_last_step = 0;
            }

            if (is_key_pressed(Keys.Down, KEY_PRESSED_TIME))
            {
                board.haste();
                board.haste_released = false;
                time_since_last_step = 0;
            }

            if (current_state.IsKeyUp(Keys.Down))
                board.haste_released = true;
           
            last_state = current_state;

            board.Update(gameTime);        
            base.Update(gameTime);
        }

        // TODO: Cannot hit keyboard fast this way
        public bool is_key_pressed(Keys key, int time_gap = 0)
        {
            return last_state.IsKeyDown(key) && current_state.IsKeyUp(key) && time_since_last_step > time_gap;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw the board and the tetrimino
            spriteBatch.Begin();

            board.Draw(spriteBatch);
          
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
