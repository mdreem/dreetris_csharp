using System;
using System.Collections.Generic;
using Dreetris.Animation;
using Dreetris.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris
{
    public class TetrisBoard
    {
        enum State
        {
            RUNNING,
            DELETING_ROWS
        }

        #region Fields

        static readonly double DELETE_TIME = 2000;

        Tetrimino.Type[,] board;
        Point position;
        int width;
        int height;
        int _level = 1;

        public int Level
        {
            get { return _level; }
            set { SetLevel(value); }
        }

        int _clearedLines;

        public int clearedLines
        {
            get { return _clearedLines; }
        }

        double timeSinceLastStep = 0;

        public double fallDelay;
        bool isHaste;
        private double fallDelayHaste = 50;

        public bool hasteReleased = true;
        bool _gameOver;

        public bool GameOver
        {
            get { return _gameOver; }
        }

        public bool showGhostTetrimino = true;

        TetriminoPreview preview;
        Tetrimino currentTetrimino;

        AssetManager assetManager;
        SoundEffect flip;
        SoundEffect deleteLine;

        Sprite block_I, block_J, block_L, block_O, block_S, block_T, block_Z;
        List<Sprite> blocks = new List<Sprite>();
        List<DissolvingSprite> dissolvingBlocks = new List<DissolvingSprite>();

        RandomBlocks randomBlocks;
        Score score = new Score();

        DKeyboard keyboard;

        State state;

        #endregion

        public TetrisBoard(AssetManager am, DKeyboard keyboard, int width, int height, int x = 0, int y = 0)
        {
            board = new Tetrimino.Type[width, height];
            position = new Point(x, y);
            this.width = width;
            this.height = height;
            this.keyboard = keyboard;
            assetManager = am;

            randomBlocks = new RandomBlocks();

            preview = new TetriminoPreview(am, randomBlocks, new Point(x + (width + 3) * Tetrimino.BLOCK_WIDTH, y));

            SetLevel(1);

            CreateTetrimino(randomBlocks.GetCurrentBlock());

            state = State.RUNNING;

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

            if (DidCollide())
                _gameOver = true;
        }

        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case State.RUNNING:
                    UpdateRunning(gameTime);
                    break;
                case State.DELETING_ROWS:
                    UpdateDeletingRows(gameTime);
                    break;
                default:
                    break;
            }
        }

        public int GetScore()
        {
            return score.score;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case State.RUNNING:
                    DrawRunning(spriteBatch);
                    break;
                case State.DELETING_ROWS:
                    DrawDeletingRows(spriteBatch);
                    break;
                default:
                    break;
            }
        }

        public void FlipTetrimino()
        {
            currentTetrimino.Flip();
            if (DidCollide())
                currentTetrimino.Unflip();
            else
                flip.Play();
        }

        public void MoveLeft()
        {
            currentTetrimino.position.X -= 1;
            if (DidCollide())
            {
                currentTetrimino.position.X += 1;
            }
        }

        public void MoveRight()
        {
            currentTetrimino.position.X += 1;
            if (DidCollide())
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

        public void Initialize()
        {
            LoadContent();
        }

        #endregion

        #region Private methods

        private void DrawRunning(SpriteBatch spriteBatch)
        {
            foreach (var b in blocks)
            {
                b.Draw(spriteBatch);
            }

            if (showGhostTetrimino)
            {
                Tetrimino ghost = GetGhost();
                ghost.SetTransparency(0.4f);
                ghost.Draw(spriteBatch);
            }

            currentTetrimino.Draw(spriteBatch);
            preview.Draw(spriteBatch);
        }

        private void DrawDeletingRows(SpriteBatch spriteBatch)
        {
            foreach (var b in blocks)
            {
                b.Draw(spriteBatch);
            }

            foreach (var b in dissolvingBlocks)
            {
                b.Draw(spriteBatch);
            }

            preview.Draw(spriteBatch);
        }

        private void UpdateRunning(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            double local_fall_delay = fallDelay;

            if (GameOver)
                return;

            timeSinceLastStep += time;

            if (isHaste)
                local_fall_delay = fallDelayHaste;

            foreach (var b in blocks)
            {
                b.Update(gameTime);
            }

            currentTetrimino.Update(gameTime);

            preview.Update(gameTime);

            if (timeSinceLastStep > local_fall_delay) // TODO: IMPORTANT: may carry over... check if it may by n times over last step
            {
                timeSinceLastStep = timeSinceLastStep - local_fall_delay;
                currentTetrimino.position.Y += 1;
                if (DidCollide())
                {
                    currentTetrimino.position.Y -= 1;
                    CopyToBoard();

                    if (DoesFullRowExist())
                    {
                        Console.WriteLine("Row full");
                        state = State.DELETING_ROWS;
                        GenerateSpriteListDeletingRows();
                        deleteLine.Play();
                    }
                    else
                    {
                        Console.WriteLine("Row not full");
                        GenerateSpriteListRunning();
                        CreateTetrimino(GetRandomType());
                    }

                    isHaste = false;
                    keyboard.LockKey(Keys.Down);
                }
            }
        }

        private void UpdateDeletingRows(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            double local_fall_delay = fallDelay;

            timeSinceLastStep += time;

            foreach (var b in blocks)
            {
                b.Update(gameTime);
            }

            foreach (var b in dissolvingBlocks)
            {
                b.update(gameTime);
            }

            preview.Update(gameTime);

            if (timeSinceLastStep > DELETE_TIME)
            {
                timeSinceLastStep -= local_fall_delay;

                int del_rows = DeleteFullRows();
                score.RowsDeleted(del_rows);
                _clearedLines += del_rows;
                Level = Gamedata.GetLevel(clearedLines);
                CreateTetrimino(GetRandomType());

                dissolvingBlocks.Clear();

                state = State.RUNNING;
            }
        }

        protected void LoadContent()
        {
            block_I = assetManager.GetSprite("block_I");
            block_J = assetManager.GetSprite("block_J");
            block_L = assetManager.GetSprite("block_L");
            block_O = assetManager.GetSprite("block_O");
            block_S = assetManager.GetSprite("block_S");
            block_T = assetManager.GetSprite("block_T");
            block_Z = assetManager.GetSprite("block_Z");

            flip = assetManager.GetSoundEffect("flip");
            deleteLine = assetManager.GetSoundEffect("del");
        }

        private Tetrimino GetGhost()
        {
            Tetrimino ghost = currentTetrimino.clone();
            int oldY = currentTetrimino.position.Y;

            while (!DidCollide())
            {
                currentTetrimino.position.Y++;
            }
            currentTetrimino.position.Y--;

            Point pos = new Point(currentTetrimino.position.X, currentTetrimino.position.Y);
            currentTetrimino.position.Y = oldY;

            ghost.position = pos;

            return ghost;
        }

        private bool DidCollide()
        {
            int[,] currentShape = currentTetrimino.GetCurrentShape();

            //i -> X axis
            //j -> Y axis
            for (int i = 0; i < currentShape.GetLength(0); i++)
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                        if (0 <= currentTetrimino.position.X + i &&
                            currentTetrimino.position.X + i < width &&
                            0 <= currentTetrimino.position.Y + j &&
                            currentTetrimino.position.Y + j < height)
                        {
                            if (board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] != Tetrimino.Type.None)
                                return true;
                        }
                        else
                            return true;
                }
            return false;
        }

        private void GenerateSpriteListRunning()
        {
            Console.WriteLine("Generate Spritelist");
            blocks.Clear();

            for (int j = 0; j < board.GetLength(1); j++)
                GenerateSpriteRow(j);
        }

        private void GenerateSpriteListDeletingRows()
        {
            Console.WriteLine("Generate Spritelist - Deleting");
            blocks.Clear();

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (IsRowFull(j))
                {
                    GenerateDissolvingSpriteRow(j);
                }
                else
                {
                    GenerateSpriteRow(j);
                }
            }
        }

        private void GenerateDissolvingSpriteRow(int j)
        {
            List<Sprite> tmpBlocks = GetBlockRow(j);

            foreach (var block in tmpBlocks)
            {
                dissolvingBlocks.Add(new DissolvingSprite(block, (float)DELETE_TIME, 0.5f * (float)DELETE_TIME, 40.0f, 40.0f, 4, 4));
            }
        }

        private void GenerateSpriteRow(int j, float transparency = 1.0f)
        {
            blocks.AddRange(GetBlockRow(j));
        }

        private List<Sprite> GetBlockRow(int j, float transparency = 1.0f)
        {
            List<Sprite> tmpBlocks = new List<Sprite>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                int x = position.X + i * Tetrimino.BLOCK_WIDTH;
                int y = position.Y + j * Tetrimino.BLOCK_HEIGHT;

                Sprite tmp;

                switch (this.board[i, j])
                {
                    case Tetrimino.Type.I:
                        tmp = block_I.Clone();
                        tmp.SetTransparency(transparency);
                        tmp.position = new Vector2(x, y);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.J:
                        tmp = block_J.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.L:
                        tmp = block_L.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.O:
                        tmp = block_O.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.S:
                        tmp = block_S.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.T:
                        tmp = block_T.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    case Tetrimino.Type.Z:
                        tmp = block_Z.Clone();
                        tmp.position = new Vector2(x, y);
                        tmp.SetTransparency(transparency);
                        tmpBlocks.Add(tmp);
                        break;
                    default:
                        break;
                }
            }

            return tmpBlocks;
        }

        private void CopyToBoard()
        {
            int[,] currentShape = currentTetrimino.GetCurrentShape();

            for (int i = 0; i < currentShape.GetLength(0); i++)
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                    {
                        if (!(0 <= currentTetrimino.position.X + i &&
                            currentTetrimino.position.X + i < width &&
                            0 <= currentTetrimino.position.Y + j &&
                            currentTetrimino.position.Y + j < height))
                        {
                            // TODO raise exeption
                        }
                        else if (board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] != Tetrimino.Type.None)
                        {
                            // TODO raise exception
                        }
                        else
                        {
                            board[currentTetrimino.position.X + i, currentTetrimino.position.Y + j] = currentTetrimino.type;
                        }
                    }
                }
        }

        private Tetrimino.Type GetRandomType()
        {
            return randomBlocks.GetCurrentBlock();
        }

        private bool DoesFullRowExist()
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (IsRowFull(i))
                    return true;
            }

            return false;
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

            GenerateSpriteListRunning();
        }

        private void ClearRow(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, row] = Tetrimino.Type.None;
            }

            GenerateSpriteListRunning();
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
