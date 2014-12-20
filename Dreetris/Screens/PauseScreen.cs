using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    class PauseScreen : Screen
    {
        const int KEY_PRESSED_TIME = 150;

        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Texture2D blank;
        
        public PauseScreen(Game game, ScreenManager screenManager) : base(game, screenManager) { }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("SpriteFont1");
            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isActive)
                return;

            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            spriteBatch.DrawString(Font1, "Paused", new Vector2(300, 120), Color.White);
            spriteBatch.DrawString(Font1, "Press Enter to Continue", new Vector2(300, 150), Color.White);

            spriteBatch.End();
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Enter))
            {
                if (screenManager.keyboard.Changed(Keys.Enter))
                {
                    unpauseScreen();
                }

                if (screenManager.keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    unpauseScreen();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }

        private void unpauseScreen()
        {
            FadeScreen fs = new FadeScreen(Game, screenManager, FadeScreen.Type.FADE_IN, 0.75f, 250);
            fs.Initialize();

            screenManager.pop();
            screenManager.push(fs);
        }
    }
}
