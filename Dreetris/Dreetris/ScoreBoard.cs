﻿using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Dreetris
{
    public class ScoreBoard
    {
        protected static int POSITION_X = 500;
        protected static int POSITION_Y = 40;

        protected AssetManager assetManager;
        protected SpriteFont font;

        private TetrisBoard board;

        public ScoreBoard(TetrisBoard board, AssetManager assetManager)
        {
            this.board = board;
            this.assetManager = assetManager;

            font = assetManager.getFont("SpriteFont1");
        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            string scoreString = "Score: " + board.GetScore().ToString();
            string levelString = "Level: " + board.level.ToString();
            string lines = "Lines: " + board.clearedLines.ToString();
            
            Color col = new Color(180, 160, 190);

            float posY = (float)POSITION_Y;

            var measurement = font.MeasureString(levelString);
            spriteBatch.DrawString(font, levelString, new Vector2(POSITION_X + 2, posY + 2), Color.Black); //Shadow
            spriteBatch.DrawString(font, levelString, new Vector2(POSITION_X, posY), col);

            posY += measurement.Y;

            measurement = font.MeasureString(scoreString);
            spriteBatch.DrawString(font, scoreString, new Vector2(POSITION_X + 2, posY + 2), Color.Black); //Shadow
            spriteBatch.DrawString(font, scoreString, new Vector2(POSITION_X, posY), col);

            posY += measurement.Y;

            measurement = font.MeasureString(lines);
            spriteBatch.DrawString(font, lines, new Vector2(POSITION_X + 2, posY + 2), Color.Black); //Shadow
            spriteBatch.DrawString(font, lines, new Vector2(POSITION_X, posY), col);
        }
    }
}
