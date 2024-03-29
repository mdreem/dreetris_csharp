﻿
using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris
{
    public class Tetrimino
    {
        #region Shape definitions

        static int[,,] Ishape = {{{1,0,0,0},
                                   {1,0,0,0},
                                   {1,0,0,0},
                                   {1,0,0,0}},
                                  {{1,1,1,1},
                                   {0,0,0,0},
                                   {0,0,0,0},
                                   {0,0,0,0}}};

        static int[,,] Jshape = {{{0,1,0,0},
                                    {0,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {0,0,1,0},
                                   {0,0,0,0}},
                                  {{0,1,1,0},
                                   {0,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{1,0,0,0},
                                   {1,1,1,0},
                                   {0,0,0,0},
                                   {0,0,0,0}}};

        static int[,,] Lshape = {{{0,1,0,0},
                                    {0,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0}},
                                  {{0,0,1,0},
                                   {1,1,1,0},
                                   {0,0,0,0},
                                   {0,0,0,0}},
                                  {{1,1,0,0},
                                   {0,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {1,0,0,0},
                                   {0,0,0,0}}};

        static int[,,] Oshape = {{{1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                   };

        static int[,,] Sshape = {{{0,1,1,0},
                                    {1,1,0,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,1,0,0},
                                   {0,1,1,0},
                                   {0,0,1,0},
                                   {0,0,0,0}}};

        static int[,,] Tshape = {{{0,1,0,0},
                                    {1,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,1,0,0},
                                   {0,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,1,0,0},
                                   {1,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}}};

        static int[,,] Zshape = {{{1,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,0,1,0},
                                   {0,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}}};
        #endregion

        #region Fields

        public enum Type { None, I, J, L, O, S, T, Z };
        int[,] currentShape = new int[4, 4];

        public static int BLOCK_WIDTH = 20;
        public static int BLOCK_HEIGHT = 20;

        public int blockWidth;
        public int blockHeight;

        float transparency = 1.0f;

        int flipState;
        int numStates;

        Tetrimino.Type _type;

        Sprite block;

        public Tetrimino.Type type
        {
            get { return _type; }
        }

        private Point coordinates;
        public Point boardPosition;
        public Point position;

        #endregion

        #region Constructors

        public Tetrimino(AssetManager am, Tetrimino.Type type)
        {
            this.block = am.GetSprite("block_" + type.ToString());

            blockWidth = BLOCK_WIDTH;
            blockHeight = BLOCK_HEIGHT;

            this._type = type;
            coordinates = new Point();
            boardPosition = new Point();
            position = new Point();

            switch (type)
            {
                case Type.I:
                    numStates = Ishape.GetLength(0);
                    break;
                case Type.J:
                    numStates = Jshape.GetLength(0);
                    break;
                case Type.L:
                    numStates = Lshape.GetLength(0);
                    break;
                case Type.O:
                    numStates = Oshape.GetLength(0);
                    break;
                case Type.S:
                    numStates = Sshape.GetLength(0);
                    break;
                case Type.T:
                    numStates = Tshape.GetLength(0);
                    break;
                case Type.Z:
                    numStates = Zshape.GetLength(0);
                    break;
                default:
                    break;
            }

            UpdateShape();
        }

        #endregion

        #region Public methods

        public void SetTransparency(float transparency = 1.0f)
        {
            this.transparency = transparency;
        }

        public void Scale(float scale = 1.0f)
        {
            blockWidth = (int)((float)BLOCK_WIDTH * scale);
            blockHeight = (int)((float)BLOCK_HEIGHT * scale);

            block.Scale(scale);
        }

        public void Update(GameTime gameTime)
        {
            block.Update(gameTime);
        }

        public void UpdateShape()
        {
            switch (_type)
            {
                case Type.I:
                    SetCurrentShape(Ishape);
                    break;
                case Type.J:
                    SetCurrentShape(Jshape);
                    break;
                case Type.L:
                    SetCurrentShape(Lshape);
                    break;
                case Type.O:
                    SetCurrentShape(Oshape);
                    break;
                case Type.S:
                    SetCurrentShape(Sshape);
                    break;
                case Type.T:
                    SetCurrentShape(Tshape);
                    break;
                case Type.Z:
                    SetCurrentShape(Zshape);
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            coordinates.X = boardPosition.X + blockWidth * position.X;
            coordinates.Y = boardPosition.Y + blockHeight * position.Y;

            for (int i = 0; i < currentShape.GetLength(0); i++)
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                    {
                        int X = coordinates.X + i * block.height;
                        int Y = coordinates.Y + j * block.width;

                        block.SetTransparency(transparency);
                        block.position = new Vector2(X, Y);
                        block.Draw(spriteBatch);
                    }
                }
        }

        public void Flip()
        {
            flipState = (flipState + 1) % numStates;
            UpdateShape();
        }

        public void Unflip()
        {
            flipState = (flipState + numStates - 1) % numStates;
            UpdateShape();
        }

        public int[,] GetCurrentShape()
        {
            return currentShape;
        }

        public Tetrimino clone()
        {
            return (Tetrimino)this.MemberwiseClone();
        }

        #endregion

        #region Private methods

        private void SetCurrentShape(int[,,] shape)
        {
            for (int i = 0; i < shape.GetLength(1); i++)
                for (int j = 0; j < shape.GetLength(2); j++)
                {
                    currentShape[i, j] = shape[flipState, j, i];
                }
        }

        #endregion
    }
}
