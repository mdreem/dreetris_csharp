using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Dreetris.Animation;

namespace Dreetris.Screens
{
    public class TitleScreen : Screen
    {
        AssetManager assetManager;

        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Sprite intro;

        public TitleScreen(Game game, ScreenManager screenManager, AssetManager assetManager) : base(game, screenManager) 
        {
            this.assetManager = assetManager;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = content.Load<SpriteFont>("SpriteFont1");

            intro = assetManager.getSprite("intro");

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
            intro.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ProcessKeyboardTitle(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Space))
            {
                screenManager.pop();
                screenManager.keyboard.LockKey(Keys.Space);
            }
            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
