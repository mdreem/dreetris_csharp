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
            Tetrimino next1 = new Tetrimino(contentManager, randomBlocks.GetBlock(1));
            Tetrimino next2 = new Tetrimino(contentManager, randomBlocks.GetBlock(2));
            Tetrimino next3 = new Tetrimino(contentManager, randomBlocks.GetBlock(3));

            current.boardPosition = position;
            next1.boardPosition = position;
            next2.boardPosition = position;
            next3.boardPosition = position;

            current.position.X = 0;
            current.position.Y = 0;

            next1.position.X = 0;
            next1.position.Y = 5;

            next2.position.X = 0;
            next2.position.Y = 10;

            next3.position.X = 0;
            next3.position.Y = 15;

            current.Draw(spriteBatch);
            next1.Draw(spriteBatch);
            next2.Draw(spriteBatch);
            next3.Draw(spriteBatch);
        }
    }
}
