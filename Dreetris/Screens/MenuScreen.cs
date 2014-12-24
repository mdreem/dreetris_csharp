using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dreetris.Screens
{
    public class MenuScreen : Screen
    {
        SpriteFont font;
        Menu menu;
        SpriteBatch spriteBatch;

        private AssetManager assetManager;
        private Texture2D blank;

        public MenuScreen(Game game, ScreenManager screenManager, AssetManager assetManager)
            : base(game, screenManager)
        {
            this.assetManager = assetManager;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = assetManager.getFont("SpriteFont1");

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            menu = new Menu();
            menu.addItem("Option1", doNothing);
            menu.addItem("Option2", doNothing);
            menu.addItem("Option3", doNothing);
        }

        protected void doNothing()
        {
            Console.WriteLine("Test");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            float originX = 400;
            float originY = 120;

            foreach (var s in menu.items)
            {
                var measurement = font.MeasureString(s);
                spriteBatch.DrawString(font, s, new Vector2(originX - measurement.X / 2, originY), Color.White);

                originY += measurement.Y;
            }

            spriteBatch.End();
        }
    }
}
