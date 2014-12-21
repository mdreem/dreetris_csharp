using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Dreetris.Animation;

namespace Dreetris
{
    public class TetrisBoard
    {
        #region Fields

        Tetrimino.Type[,] board;
        Point position;
        int width;
        int height;

        TetriminoPreview preview;

        SoundEffect flip;

        int _level = 1;

        public int level
        {
            get { return _level; }
            set { SetLevel(value); }
        }

        public bool showGhostTetrimino = true;

        Random random = new Random();
        RandomBlocks randomBlocks;
        Score score = new Score();

        Sprite block_I, block_J, block_L, block_O,
               block_S, block_T, block_Z;

        List<Sprite> blocks = new List<Sprite>();

        //Texture2D sprite;
        Rectangle drawRectangle;

        Tetrimino currentTetrimino;

        AssetManager assetManager;

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

        public TetrisBoard(AssetManager am, DKeyboard keyboard, int width, int height, int x = 0, int y = 0)
        {
            board = new Tetrimino.Type[width, height];
            position = new Point(x, y);
            this.width = width;
            this.height = height;
            this.keyboard = keyboard;
            assetManager = am;

            drawRectangle = new Rectangle(0, 0, Tetrimino.BLOCK_WIDTH, Tetrimino.BLOCK_HEIGHT);

            //InitializeBackground(contentManager);
            //LoadContent(contentManager);

            randomBlocks = new RandomBlocks();

            preview = new TetriminoPreview(am, randomBlocks, new Point(x + (width + 3) * Tetrimino.BLOCK_WIDTH, y));

            SetLevel(1);

            CreateTetrimino(randomBlocks.GetCurrentBlock());

            System.Diagnostics.Debug.WriteLine("Types: " + Enum.GetValues(typeof(Tetrimino.Type)).ToString());
        }

        #region Public methods

        public void CreateTetrimino(Tetrimino.Type type)
        {
            System.Diagnostics.Debug.WriteLine("New Tetrimino: " + type.ToString());

            currentTetrimino = new Tetrimino(assetManager, type);
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

            //Update the blocks on the field
            foreach (var b in blocks)
            {
                b.update(gameTime);
            }

            //update the current Tetrimino
            currentTetrimino.update(gameTime);

            //update the preview-element
            preview.update(gameTime);

            //           System.Diagnostics.Debug.WriteLine("Speed: " + local_fall_delay.ToString());

            /* Let the Tetrimino fall until it hits something, then copy it onto the board
             * update every fall_delay ms.
             * */
            if (timeSinceLastStep > local_fall_delay) // TODO: IMPORTANT: may carry over... check if it may by n times over last step
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
            foreach(var b in blocks)
            {
                b.draw(spriteBatch);
            }

            if (showGhostTetrimino)
            {
                Tetrimino ghost = getGhost();
                ghost.setTransparency(0.4f);
                ghost.draw(spriteBatch);
            }

            currentTetrimino.draw(spriteBatch);
            preview.draw(spriteBatch);
        }

        /// <summary>
        /// Flips the Tetrimino.
        /// </summary>
        public void FlipTetrimino()
        {
            currentTetrimino.Flip();
            if (HasCollided())
                currentTetrimino.Unflip();
            else
                flip.Play();
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

        public void Initialize()
        {
            LoadContent();
        }

        protected void LoadContent()
        {
            // load content and set remainder of draw rectangle

            block_I = assetManager.getSprite("block_I");
            block_J = assetManager.getSprite("block_J");
            block_L = assetManager.getSprite("block_L");
            block_O = assetManager.getSprite("block_O");
            block_S = assetManager.getSprite("block_S");
            block_T = assetManager.getSprite("block_T");
            block_Z = assetManager.getSprite("block_Z");

            drawRectangle = new Rectangle(0, 0, 20, 20);

            flip = assetManager.getSoundEffect("flip");
        }

        #region Private methods

        /// <summary>
        /// Initialization of the board's background.
        /// </summary>
        private void InitializeBackground(ContentManager contentManager)
        {
        }

        private Tetrimino getGhost()
        {
            Tetrimino ghost = currentTetrimino.clone();
            int oldY = currentTetrimino.position.Y;

            while (!HasCollided())
            {
                currentTetrimino.position.Y++;
            }
            currentTetrimino.position.Y--;

            Point pos = new Point(currentTetrimino.position.X, currentTetrimino.position.Y);
            currentTetrimino.position.Y = oldY;

            ghost.position = pos;

            return ghost;
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

        private void generateSpriteList()
        {
            Console.WriteLine("Generate Spritelist");
            blocks.Clear();

            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int x = position.X + i * Tetrimino.BLOCK_WIDTH;
                    int y = position.Y + j * Tetrimino.BLOCK_HEIGHT;

                    Sprite tmp;

                    switch (this.board[i, j])
                    {
                        case Tetrimino.Type.I:
                            tmp = block_I.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.J:
                            tmp = block_J.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.L:
                            tmp = block_L.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.O:
                            tmp = block_O.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.S:
                            tmp = block_S.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.T:
                            tmp = block_T.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        case Tetrimino.Type.Z:
                            tmp = block_Z.Clone();
                            tmp.position = new Vector2(x, y);
                            blocks.Add(tmp);
                            break;
                        default:
                            break;
                    }
                }
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
                            board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] = currentTetrimino.type;
                        }
                    }
                }

            generateSpriteList();
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

            generateSpriteList();
        }

        private void ClearRow(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, row] = Tetrimino.Type.None;
            }

            generateSpriteList();
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
