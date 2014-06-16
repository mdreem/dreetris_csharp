using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Dreetris
{
    public class TetriminoPreview
    {
        RandomBlocks randomBlocks;
        Point position;
        ContentManager contentManager;

        public TetriminoPreview(RandomBlocks randomBlocks, ContentManager contentManager, Point position)
        {
            this.randomBlocks = randomBlocks;
            this.position = position;
            this.contentManager = contentManager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Tetrimino current = new Tetrimino(contentManager, randomBlocks.GetBlock(0));
            Tetrimino next1 = new Tetrimino(contentManager, randomBlocks.GetBlock(1), 0.7f);
            Tetrimino next2 = new Tetrimino(contentManager, randomBlocks.GetBlock(2), 0.7f);
            Tetrimino next3 = new Tetrimino(contentManager, randomBlocks.GetBlock(3), 0.7f);

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
            next3.Draw(spriteBatch);
        }
    }
}
