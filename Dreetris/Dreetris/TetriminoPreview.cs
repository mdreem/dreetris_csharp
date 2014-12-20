using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Dreetris.Animation;

namespace Dreetris
{
    public class TetriminoPreview
    {
        RandomBlocks randomBlocks;
        Point position;
        AssetManager assetManager;

        // has a copy of all Tetriminos loaded and moves them to the right part
        // of the screen when draw(...) is called.
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
            // load content and set remainder of draw rectangle

            block_I = new Tetrimino(assetManager, Tetrimino.Type.I);
            block_J = new Tetrimino(assetManager, Tetrimino.Type.J);
            block_L = new Tetrimino(assetManager, Tetrimino.Type.L);
            block_O = new Tetrimino(assetManager, Tetrimino.Type.O);
            block_S = new Tetrimino(assetManager, Tetrimino.Type.S);
            block_T = new Tetrimino(assetManager, Tetrimino.Type.T);
            block_Z = new Tetrimino(assetManager, Tetrimino.Type.Z);
        }

        protected Tetrimino getTetrimino(Tetrimino.Type type)
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

        public void update(GameTime gameTime)
        {
            //needed for animating the blocks
            block_I.update(gameTime);
            block_J.update(gameTime);
            block_L.update(gameTime);
            block_O.update(gameTime);
            block_S.update(gameTime);
            block_T.update(gameTime);
            block_Z.update(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            // the next four Tetriminos
            Tetrimino current = getTetrimino(randomBlocks.GetBlock(0));
            Tetrimino next1 = getTetrimino(randomBlocks.GetBlock(1));
            Tetrimino next2 = getTetrimino(randomBlocks.GetBlock(2));
            Tetrimino next3 = getTetrimino(randomBlocks.GetBlock(3));

            current.boardPosition = position;

            current.position.X = 0;
            current.position.Y = 0;

            current.scale();
            current.draw(spriteBatch);

            Point position2 = new Point();
            position2.X = position.X;
            position2.Y = position.Y + Tetrimino.BLOCK_HEIGHT * 5;

            // relative position of the scaled Tetriminos
            next1.boardPosition = position2;
            next2.boardPosition = position2;
            next3.boardPosition = position2;

            // draw the scaled Tritriminos onto the screen

            next1.scale(0.75f);
            next2.scale(0.75f);
            next3.scale(0.75f);

            next1.position.X = 0;
            next1.position.Y = 0;

            next2.position.X = 0;
            next2.position.Y = 5;

            next3.position.X = 0;
            next3.position.Y = 10;

            next1.draw(spriteBatch);
            next2.draw(spriteBatch);
            next3.draw(spriteBatch);
        }
    }
}
