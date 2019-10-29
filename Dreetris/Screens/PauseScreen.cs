using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    class PauseScreen : MenuScreen
    {
        protected const int TITLE_X = 400;
        protected const int TITLE_Y = 100;

        SpriteFont titleFont;

        public PauseScreen(GameObjects gameObjects)
            : base(gameObjects)
        {
            MenuEntryText me1 = new MenuEntryText(gameObjects, "Resume", UnpauseScreen);
            MenuEntryText me2 = new MenuEntryText(gameObjects, "Options", OpenOptionsMenu);
            MenuEntryText me3 = new MenuEntryText(gameObjects, "Restart", RestartGame);

            menu.AddItem(me1);
            menu.AddItem(me2);
            menu.AddItem(me3);

            originX = 400;
            originY = 200;

            titleFont = assetManager.GetFont("TitleFont");
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isActive)
                return;

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
            if (screenManager.keyboard.downRepeated(Keys.Enter, 3 * KEY_PRESSED_TIME))
            {
                UnpauseScreen();
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }

        private void UnpauseScreen()
        {
            FadeScreen fs = new FadeScreen(gameObjects, FadeScreen.Type.FADE_IN, 0.75f, 250);
            fs.Initialize();

            screenManager.Pop();
            screenManager.Push(fs);
        }

        private void OpenOptionsMenu()
        {
            OptionsScreen os = new OptionsScreen(gameObjects);
            screenManager.Push(os);
        }

        private void RestartGame()
        {
            //Remove alle screens
            screenManager.Pop();
            screenManager.Pop();

            FadeScreen fs1 = new FadeScreen(gameObjects, FadeScreen.Type.FADE_OUT);
            fs1.Initialize();

            FadeScreen fs2 = new FadeScreen(gameObjects, FadeScreen.Type.FADE_IN);
            fs2.Initialize();

            GameScreen gs = new GameScreen(gameObjects);
            gs.Initialize();

            FadeScreen fs3 = new FadeScreen(gameObjects, FadeScreen.Type.FADE_IN);
            fs3.Initialize();

            TitleScreen ts = new TitleScreen(gameObjects);
            ts.Initialize();

            screenManager.Push(gs);
            screenManager.Push(fs3);
            screenManager.Push(ts);
            screenManager.Push(fs2);
            screenManager.Push(fs1); //TODO: Working?
        }
    }
}
