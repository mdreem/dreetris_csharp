using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris
{
    class TetrisBoard
    {
        #region Fields

        Tetrimino.Type[,] board;
        Point position;
        int width;
        int height;

        ContentManager content;
        Random random = new Random();
        RandomBlocks random_blocks;
        Score score = new Score();

        Texture2D sprite;
        Rectangle draw_rectangle;

        Tetrimino current_tetrimino;

        double time_since_last_step = 0;

        public double fall_delay = 500;  // Delay in ms until a tetrimino changes moves one step
        bool is_haste = false;
        private double fall_delay_haste = 50;

        public bool haste_released = true;

        #endregion

        public TetrisBoard(ContentManager contentManager, int width, int height, int x = 0, int y = 0)
        {
            board = new Tetrimino.Type[width, height];
            position = new Point(x, y);
            this.width = width;
            this.height = height;

            draw_rectangle = new Rectangle(0, 0, Tetrimino.BLOCK_WIDTH, Tetrimino.BLOCK_HEIGHT);

            InitializeBackground(contentManager);
            LoadContent(contentManager);

            content = contentManager;
            random_blocks = new RandomBlocks(3);
            //            Enum.GetValues(typeof(Tetrimino.Type));

            System.Diagnostics.Debug.WriteLine("Types: " + Enum.GetValues(typeof(Tetrimino.Type)).ToString());
        }

        #region Public methods

        public void CreateTetrimino(Tetrimino.Type type)
        {
            System.Diagnostics.Debug.WriteLine("New Tetrimino: " + type.ToString());
            current_tetrimino = new Tetrimino(content, type);
            current_tetrimino.position.X = width / 2;
            current_tetrimino.position.Y = 0;
            current_tetrimino.board_position = position;
        }

        /// <summary>
        /// Updates the current board
        /// </summary>
        public void Update(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            double local_fall_delay = fall_delay;

            time_since_last_step += time;

            if (is_haste)
                local_fall_delay = fall_delay_haste;

            //           System.Diagnostics.Debug.WriteLine("Speed: " + local_fall_delay.ToString());

            /* Let the Tetrimino fall until it hits something, then copy it onto the board
             * update every fall_delay ms.
             * */
            if (time_since_last_step > local_fall_delay) // IMPORTANT: may carry over... check if it may by n times over last step
            {
                time_since_last_step = time_since_last_step - local_fall_delay;
                current_tetrimino.position.Y += 1;
                if (has_collided())
                {
                    current_tetrimino.position.Y -= 1;
                    copy_to_board();
                    score.rows_deleted(delete_full_rows());
                    CreateTetrimino(get_random_type());
                    is_haste = false;
                }
            }
        }

        public int get_score()
        {
            return score.get_score();
        }

        /// <summary>
        /// Draw the board together with the current Tetrimino on the screen.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // Construct pixel coordinates from Positions and Tetrimino-Data
                    draw_rectangle.X = position.X + i * Tetrimino.BLOCK_WIDTH;
                    draw_rectangle.Y = position.Y + j * Tetrimino.BLOCK_HEIGHT;

                    switch (this.board[i, j])
                    {
                        case Tetrimino.Type.I:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Cyan);
                            break;
                        case Tetrimino.Type.J:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.DarkBlue);
                            break;
                        case Tetrimino.Type.L:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Orange);
                            break;
                        case Tetrimino.Type.O:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Yellow);
                            break;
                        case Tetrimino.Type.S:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Green);
                            break;
                        case Tetrimino.Type.T:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Purple);
                            break;
                        case Tetrimino.Type.Z:
                            spriteBatch.Draw(sprite, draw_rectangle, Color.Red);
                            break;
                        default:
                            break;
                    }
                }
            current_tetrimino.Draw(spriteBatch);
        }

        /// <summary>
        /// Flips the Tetrimino.
        /// </summary>
        public void flip_tetrimino()
        {
            current_tetrimino.flip();
            if (has_collided())
                current_tetrimino.unflip();
        }

        /// <summary>
        /// Moves the Tetrimino to the left.
        /// </summary>
        public void move_left()
        {
            current_tetrimino.position.X -= 1;
            if (has_collided())
            {
                current_tetrimino.position.X += 1;
            }
        }

        /// <summary>
        /// Moves the Tetrimino to the right.
        /// </summary>
        public void move_right()
        {
            current_tetrimino.position.X += 1;
            if (has_collided())
            {
                current_tetrimino.position.X -= 1;
            }
        }

        public void haste()
        {
            if (haste_released)
                is_haste = true;
        }

        public RandomBlocks get_randomizer()
        {
            return random_blocks;
        }

        #endregion

        #region Private methods

        private void LoadContent(ContentManager contentManager)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>("block");
            draw_rectangle = new Rectangle(0, 0, 20, 20);
        }

        /// <summary>
        /// Initialization of the board's background.
        /// </summary>
        private void InitializeBackground(ContentManager contentManager)
        {
            //background_image = contentManager.Load<Texture2D>("background");
            //background_image = contentManager.Load<Texture2D>("block");
            /*
            background_rectangle = new Rectangle(position.X, position.Y,
                position.X + Tetrimino.BLOCK_WIDTH * (this.width - 1),
                position.Y + Tetrimino.BLOCK_HEIGHT * (this.height - 1));*/
        }

        /// <summary>
        /// Check for a collision of the Tetrimino with elements on the board or the board's borders.
        /// </summary>
        private bool has_collided()
        {
            // for every element in the current tetrimino
            int[,] current_shape = current_tetrimino.getCurrentShape();

            //i -> X axis
            //j -> Y axis
            for (int i = 0; i < current_shape.GetLength(0); i++)
                for (int j = 0; j < current_shape.GetLength(1); j++)
                {
                    // Check if the block is inside the boundaries of the board return collision if not
                    if (current_shape[i, j] == 1)
                        if (0 <= current_tetrimino.position.X + i &&
                            current_tetrimino.position.X + i < width &&
                            0 <= current_tetrimino.position.Y + j &&
                            current_tetrimino.position.Y + j < height)
                        {
                            // Check for collisions agains Blocks already on the board
                            if (board[current_tetrimino.position.X + i, current_tetrimino.position.Y + j] != Tetrimino.Type.None)
                                return true;
                        }
                        else
                            return true;
                }
            return false;
        }

        /// <summary>
        /// Copy the current Tetrimino to the board.
        /// </summary>
        private void copy_to_board()
        {
            // for every element in the current tetrimino
            int[,] current_shape = current_tetrimino.getCurrentShape();

            for (int i = 0; i < current_shape.GetLength(0); i++)
                for (int j = 0; j < current_shape.GetLength(1); j++)
                {
                    if (current_shape[i, j] == 1)
                    {
                        // Check if the block is inside the boundaries of the board before copying.
                        if (!(0 <= current_tetrimino.position.X + i &&
                            current_tetrimino.position.X + i < width &&
                            0 <= current_tetrimino.position.Y + j &&
                            current_tetrimino.position.Y + j < height))
                        {
                            // raise exeption
                        }
                        else if (board[current_tetrimino.position.X + i, current_tetrimino.position.Y + j] != Tetrimino.Type.None)
                        {
                            // raise exception
                        }
                        else
                        {
                            board[current_tetrimino.position.X + i, current_tetrimino.position.Y + j] = current_tetrimino.get_type();
                        }
                    }
                }
        }

        private Tetrimino.Type get_random_type()
        {
            return random_blocks.get_current_block();
        }

        private bool is_row_full(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, row] == Tetrimino.Type.None)
                    return false;
            }

            return true;
        }

        private void copy_row(int from_row, int to_row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, to_row] = board[i, from_row];
            }
        }

        private void clear_row(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, row] = Tetrimino.Type.None;
            }
        }

        private void delete_and_move_row(int row)
        {
            //TODO: Check if row exists!!
            for (int i = row; i > 0; i--)
            {
                copy_row(i - 1, i);
            }
            clear_row(0);
        }

        private int delete_full_rows()
        {
            int row_count = 0;
            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (is_row_full(i))
                {
                    delete_and_move_row(i);
                    row_count++;
                }
            }
            return row_count;
        }

        #endregion
    }
}
