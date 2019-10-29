using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    public class TitleScreen : Screen
    {
        AssetManager assetManager;

        SpriteBatch spriteBatch;
        Sprite intro;

        public TitleScreen(GameObjects gameObjects)
            : base(gameObjects) 
        {
            this.assetManager = gameObjects.AssetManager;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            intro = assetManager.GetSprite("intro");

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
            spriteBatch.Begin();
            intro.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ProcessKeyboardTitle(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Space))
            {
                FadeScreen fs = new FadeScreen(gameObjects, FadeScreen.Type.FADE_OUT_POP);
                fs.Initialize();
                screenManager.Push(fs);
                screenManager.keyboard.LockKey(Keys.Space);
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
