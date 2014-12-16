using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    public class TitleScreen : Screen
    {
        SpriteBatch spriteBatch;
        SpriteFont Font1;

        public TitleScreen(Game game, ScreenManager screenManager) : base(game, screenManager) { }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("SpriteFont1");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);

            ProcessKeyboardTitle(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.DrawString(Font1, "Dreetris", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Space to Start", new Vector2(300, 150), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ProcessKeyboardTitle(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Space))
            {
                screenManager.pop();
                //gamestate = State.RUNNING;   !!!
                //InitializeGame();  !!
                screenManager.keyboard.LockKey(Keys.Space);
            }
            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
