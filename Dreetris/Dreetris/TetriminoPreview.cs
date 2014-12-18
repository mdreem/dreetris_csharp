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

        Sprite block_I, block_J, block_L, block_O,
               block_S, block_T, block_Z;

        public TetriminoPreview(AssetManager assetManager, RandomBlocks randomBlocks, Point position)
        {
            this.assetManager = assetManager;
            this.randomBlocks = randomBlocks;
            this.position = position;
        }

        protected void LoadContent()
        {
            // load content and set remainder of draw rectangle
            //sprite = content.Load<Texture2D>("block");

            block_I = assetManager.getSprite("block_I");
            block_J = assetManager.getSprite("block_J");
            block_L = assetManager.getSprite("block_L");
            block_O = assetManager.getSprite("block_O");
            block_S = assetManager.getSprite("block_S");
            block_T = assetManager.getSprite("block_T");
            block_Z = assetManager.getSprite("block_Z");
        }

        public void Draw(SpriteBatch spriteBatch)
        {/*
            Tetrimino current = new Tetrimino(am, randomBlocks.GetBlock(0));
            Tetrimino next1 = new Tetrimino(am, randomBlocks.GetBlock(1), 0.7f);
            Tetrimino next2 = new Tetrimino(am, randomBlocks.GetBlock(2), 0.7f);
            Tetrimino next3 = new Tetrimino(am, randomBlocks.GetBlock(3), 0.7f);

            Point position2 = new Point();
            position2.X = position.X;
            position2.Y = position.Y + Tetrimino.BLOCK_HEIGHT * 5;

            current.boardPosition = position;
            next1.boardPosition = position2;
            next2.boardPosition = position2;
            next3.boardPosition = position2;

            current.position.X = 0;
            current.position.Y = 0;

            next1.position.X = 0;
            next1.position.Y = 0;

            next2.position.X = 0;
            next2.position.Y = 5;

            next3.position.X = 0;
            next3.position.Y = 10;

            current.Draw(spriteBatch);
            next1.Draw(spriteBatch);
            next2.Draw(spriteBatch);
            next3.Draw(spriteBatch);*/
        }
    }
}
