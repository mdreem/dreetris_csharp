using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris
{
    public class TetriminoPreview
    {
        RandomBlocks randomBlocks;
        Point position;
        AssetManager assetManager;

        Tetrimino block_I, block_J, block_L, block_O,
               block_S, block_T, block_Z;

        public TetriminoPreview(AssetManager assetManager, RandomBlocks randomBlocks, Point position)
        {
            this.assetManager = assetManager;
            this.randomBlocks = randomBlocks;
            this.position = position;

            LoadContent();
        }

        protected void LoadContent()
        {
            block_I = new Tetrimino(assetManager, Tetrimino.Type.I);
            block_J = new Tetrimino(assetManager, Tetrimino.Type.J);
            block_L = new Tetrimino(assetManager, Tetrimino.Type.L);
            block_O = new Tetrimino(assetManager, Tetrimino.Type.O);
            block_S = new Tetrimino(assetManager, Tetrimino.Type.S);
            block_T = new Tetrimino(assetManager, Tetrimino.Type.T);
            block_Z = new Tetrimino(assetManager, Tetrimino.Type.Z);
        }

        protected Tetrimino GetTetrimino(Tetrimino.Type type)
        {
            switch (type)
            {
                case Tetrimino.Type.I:
                    return block_I;
                case Tetrimino.Type.J:
                    return block_J;
                case Tetrimino.Type.L:
                    return block_L;
                case Tetrimino.Type.O:
                    return block_O;
                case Tetrimino.Type.S:
                    return block_S;
                case Tetrimino.Type.T:
                    return block_T;
                case Tetrimino.Type.Z:
                    return block_Z;
                default:
                    return null;
            }
        }

        public void Update(GameTime gameTime)
        {
            block_I.Update(gameTime);
            block_J.Update(gameTime);
            block_L.Update(gameTime);
            block_O.Update(gameTime);
            block_S.Update(gameTime);
            block_T.Update(gameTime);
            block_Z.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Tetrimino current = GetTetrimino(randomBlocks.GetBlock(0));
            Tetrimino next1 = GetTetrimino(randomBlocks.GetBlock(1));
            Tetrimino next2 = GetTetrimino(randomBlocks.GetBlock(2));
            Tetrimino next3 = GetTetrimino(randomBlocks.GetBlock(3));

            current.boardPosition = position;

            current.position.X = 0;
            current.position.Y = 0;

            current.Scale();
            current.Draw(spriteBatch);

            Point position2 = new Point();
            position2.X = position.X;
            position2.Y = position.Y + Tetrimino.BLOCK_HEIGHT * 5;

            next1.boardPosition = position2;
            next2.boardPosition = position2;
            next3.boardPosition = position2;

            next1.Scale(0.75f);
            next2.Scale(0.75f);
            next3.Scale(0.75f);

            next1.position.X = 0;
            next1.position.Y = 0;

            next2.position.X = 0;
            next2.position.Y = 5;

            next3.position.X = 0;
            next3.position.Y = 10;

            next1.Draw(spriteBatch);
            next2.Draw(spriteBatch);
            next3.Draw(spriteBatch);
        }
    }
}
