using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Dreetris.Screens
{
    class PauseScreen : MenuScreen
    {
        protected const int TITLE_X = 400;
        protected const int TITLE_Y = 100;

        SpriteFont titleFont;

        public PauseScreen(Game game, ScreenManager screenManager, AssetManager assetManager)
            : base(game, screenManager, assetManager)
        {
            menu.addItem("Resume", unpauseScreen);
            menu.addItem("Options", openOptionsMenu);
            menu.addItem("Restart", restartGame);

            originX = 400;
            originY = 200;

            titleFont = assetManager.getFont("TitleFont");
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isActive)
                return;

            //draw the menu itself
            base.Draw(gameTime);

            spriteBatch.Begin();

            Color col = new Color(180, 160, 190);
            string s = "Game Paused";

            var measurement = titleFont.MeasureString(s);
            spriteBatch.DrawString(titleFont, s, new Vector2(TITLE_X - measurement.X / 2 + 2, TITLE_Y + 2), Color.Black); //Shadow
            spriteBatch.DrawString(titleFont, s, new Vector2(TITLE_X - measurement.X / 2, TITLE_Y), col);

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

        private void openOptionsMenu()
        {

        }

        private void restartGame()
        {
            //Remove alle screens
            screenManager.pop();
            screenManager.pop();

            FadeScreen fs1 = new FadeScreen(Game, screenManager, FadeScreen.Type.FADE_OUT);
            fs1.Initialize();

            FadeScreen fs2 = new FadeScreen(Game, screenManager, FadeScreen.Type.FADE_IN);
            fs2.Initialize();

            GameScreen gs = new GameScreen(Game, screenManager, assetManager);
            gs.Initialize();

            FadeScreen fs3 = new FadeScreen(Game, screenManager, FadeScreen.Type.FADE_IN);
            fs3.Initialize();

            TitleScreen ts = new TitleScreen(Game, screenManager, assetManager);
            ts.Initialize();

            screenManager.push(gs);
            screenManager.push(fs3);
            screenManager.push(ts);
            screenManager.push(fs2);
            screenManager.push(fs1); //TODO: Working?
        }
    }
}
