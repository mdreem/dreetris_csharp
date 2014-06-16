using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris
{
    class TetrisBoard
    {
        #region Fields

        Tetrimino.Type[,] board;
        Point position;
        int width;
        int height;

        int _level = 1;

        public int level
        {
            get { return _level; }
            set { SetLevel(value); }
        }

        ContentManager content;
        Random random = new Random();
        RandomBlocks randomBlocks;
        Score score = new Score();

        Texture2D sprite;
        Rectangle drawRectangle;

        Tetrimino currentTetrimino;

        int _clearedLines = 0;

        public int clearedLines
        {
            get { return _clearedLines; }
        }

        DKeyboard keyboard;

        double timeSinceLastStep = 0;

        public double fallDelay;  // Delay in ms until a tetrimino changes moves one step
        bool isHaste = false;
        private double fallDelayHaste = 50;

        public bool hasteReleased = true;
        bool _gameOver = false;

        public bool gameOver
        {
            get { return _gameOver; }
        }

        #endregion

        public TetrisBoard(ContentManager contentManager, DKeyboard keyboard, int width, int height, int x = 0, int y = 0)
        {
            board = new Tetrimino.Type[width, height];
            position = new Point(x, y);
            this.width = width;
            this.height = height;
            this.keyboard = keyboard;

            drawRectangle = new Rectangle(0, 0, Tetrimino.BLOCK_WIDTH, Tetrimino.BLOCK_HEIGHT);

            InitializeBackground(contentManager);
            LoadContent(contentManager);

            content = contentManager;
            randomBlocks = new RandomBlocks();
            //            Enum.GetValues(typeof(Tetrimino.Type));

            SetLevel(1);
            
            System.Diagnostics.Debug.WriteLine("Types: " + Enum.GetValues(typeof(Tetrimino.Type)).ToString());
        }

        #region Public methods

        public void CreateTetrimino(Tetrimino.Type type)
        {
            System.Diagnostics.Debug.WriteLine("New Tetrimino: " + type.ToString());
            currentTetrimino = new Tetrimino(content, type);
            currentTetrimino.position.X = width / 2;
            currentTetrimino.position.Y = 0;
            currentTetrimino.boardPosition = position;

            if (HasCollided())
                _gameOver = true;
        }

        /// <summary>
        /// Updates the current board
        /// </summary>
        public void Update(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            double local_fall_delay = fallDelay;

            if (gameOver)
                return;

            timeSinceLastStep += time;

            if (isHaste)
                local_fall_delay = fallDelayHaste;

            //           System.Diagnostics.Debug.WriteLine("Speed: " + local_fall_delay.ToString());

            /* Let the Tetrimino fall until it hits something, then copy it onto the board
             * update every fall_delay ms.
             * */
            if (timeSinceLastStep > local_fall_delay) // IMPORTANT: may carry over... check if it may by n times over last step
            {
                timeSinceLastStep = timeSinceLastStep - local_fall_delay;
                currentTetrimino.position.Y += 1;
                if (HasCollided())
                {
                    currentTetrimino.position.Y -= 1;
                    CopyToBoard();
                    int del_rows = DeleteFullRows();
                    score.RowsDeleted(del_rows);
                    _clearedLines += del_rows;
                    level = Gamedata.GetLevel(clearedLines);
                    CreateTetrimino(GetRandomType());
                    isHaste = false;
                    keyboard.LockKey(Keys.Down);
                }
            }
        }

        public int GetScore()
        {
            return score.score;
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
                    drawRectangle.X = position.X + i * Tetrimino.BLOCK_WIDTH;
                    drawRectangle.Y = position.Y + j * Tetrimino.BLOCK_HEIGHT;

                    switch (this.board[i, j])
                    {
                        case Tetrimino.Type.I:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Cyan);
                            break;
                        case Tetrimino.Type.J:
                            spriteBatch.Draw(sprite, drawRectangle, Color.DarkBlue);
                            break;
                        case Tetrimino.Type.L:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Orange);
                            break;
                        case Tetrimino.Type.O:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Yellow);
                            break;
                        case Tetrimino.Type.S:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Green);
                            break;
                        case Tetrimino.Type.T:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Purple);
                            break;
                        case Tetrimino.Type.Z:
                            spriteBatch.Draw(sprite, drawRectangle, Color.Red);
                            break;
                        default:
                            break;
                    }
                }
            currentTetrimino.Draw(spriteBatch);
        }

        /// <summary>
        /// Flips the Tetrimino.
        /// </summary>
        public void FlipTetrimino()
        {
            currentTetrimino.Flip();
            if (HasCollided())
                currentTetrimino.Unflip();
        }

        /// <summary>
        /// Moves the Tetrimino to the left.
        /// </summary>
        public void MoveLeft()
        {
            currentTetrimino.position.X -= 1;
            if (HasCollided())
            {
                currentTetrimino.position.X += 1;
            }
        }

        /// <summary>
        /// Moves the Tetrimino to the right.
        /// </summary>
        public void MoveRight()
        {
            currentTetrimino.position.X += 1;
            if (HasCollided())
            {
                currentTetrimino.position.X -= 1;
            }
        }

        public void Haste()
        {
            if (hasteReleased)
                isHaste = true;
        }

        public void Unhaste()
        {
            if (hasteReleased)
                isHaste = false;
        }

        public RandomBlocks GetRandomizer()
        {
            return randomBlocks;
        }

        public void SetLevel(int level)
        {
            this._level = level;
            fallDelay = Gamedata.GetFallingSpeed(level);
            System.Console.WriteLine("Level: {0}", level);
            System.Console.WriteLine("Falling speed: {0}", fallDelay);
        }

        public void NextLevel()
        {
            SetLevel(_level + 1);
        }

        #endregion

        #region Private methods

        private void LoadContent(ContentManager contentManager)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>("block");
            drawRectangle = new Rectangle(0, 0, 20, 20);
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
        private bool HasCollided()
        {
            // for every element in the current tetrimino
            int[,] currentShape = currentTetrimino.GetCurrentShape();

            //i -> X axis
            //j -> Y axis
            for (int i = 0; i < currentShape.GetLength(0); i++)
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    // Check if the block is inside the boundaries of the board return collision if not
                    if (currentShape[i, j] == 1)
                        if (0 <= currentTetrimino.position.X + i &&
                            currentTetrimino.position.X + i < width &&
                            0 <= currentTetrimino.position.Y + j &&
                            currentTetrimino.position.Y + j < height)
                        {
                            // Check for collisions agains Blocks already on the board
                            if (board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] != Tetrimino.Type.None)
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
        private void CopyToBoard()
        {
            // for every element in the current tetrimino
            int[,] currentShape = currentTetrimino.GetCurrentShape();

            for (int i = 0; i < currentShape.GetLength(0); i++)
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                    {
                        // Check if the block is inside the boundaries of the board before copying.
                        if (!(0 <= currentTetrimino.position.X + i &&
                            currentTetrimino.position.X + i < width &&
                            0 <= currentTetrimino.position.Y + j &&
                            currentTetrimino.position.Y + j < height))
                        {
                            // raise exeption
                        }
                        else if (board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] != Tetrimino.Type.None)
                        {
                            // raise exception
                        }
                        else
                        {
                            board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] = currentTetrimino.GetType();
                        }
                    }
                }
        }

        private Tetrimino.Type GetRandomType()
        {
            return randomBlocks.GetCurrentBlock();
        }

        private bool IsRowFull(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, row] == Tetrimino.Type.None)
                    return false;
            }
            return true;
        }

        private void CopyRow(int fromRow, int toRow)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, toRow] = board[i, fromRow];
            }
        }

        private void ClearRow(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, row] = Tetrimino.Type.None;
            }
        }

        private void DeleteAndMoveRow(int row)
        {
            //TODO: Check if row exists!!
            for (int i = row; i > 0; i--)
            {
                CopyRow(i - 1, i);
            }
            ClearRow(0);
        }

        private int DeleteFullRows()
        {
            int rowCount = 0;
            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (IsRowFull(i))
                {
                    DeleteAndMoveRow(i);
                    rowCount++;
                }
            }
            return rowCount;
        }

        #endregion
    }
}
