using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dreetris.Screens
{
    public class MenuScreen : Screen
    {
        const int KEY_PRESSED_TIME = 150;

        SpriteFont font;
        Menu menu;
        SpriteBatch spriteBatch;
        Sprite pointerLeft;
        Sprite pointerRight;

        SoundEffect move;

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

            pointerLeft = assetManager.getSprite("button");
            pointerRight = assetManager.getSprite("button");

            pointerLeft.centerCoordinates();
            pointerRight.centerCoordinates();

            menu = new Menu();
            menu.addItem("Option1", doNothing);
            menu.addItem("Option2", doNothing);
            menu.addItem("Option3", doNothing);

            move = assetManager.getSoundEffect("selection");
        }

        protected void doNothing()
        {
            Console.WriteLine("Test");
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            pointerLeft.update(gameTime);
            pointerRight.update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            float originX = 400;
            float originY = 120;

            float menuWidth = menu.getWidth(font);
            float itemHeight = font.MeasureString(menu.items[menu.getSelected()]).Y;


            //TODO: genauer, falls Höhen Unterschiedlich
            pointerLeft.position = new Vector2((originX - menuWidth * 1.20f), originY + itemHeight * menu.getSelected() + itemHeight / 2);
            pointerRight.position = new Vector2((originX + menuWidth * 1.20f), originY + itemHeight * menu.getSelected() + itemHeight / 2);

            foreach (var s in menu.items)
            {
                var measurement = font.MeasureString(s);
                spriteBatch.DrawString(font, s, new Vector2(originX - measurement.X / 2, originY), Color.White);

                originY += measurement.Y;
            }

            pointerLeft.draw(spriteBatch);
            pointerRight.draw(spriteBatch);

            spriteBatch.End();
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (screenManager.keyboard.IsDown(Keys.Down))
            {
                if (screenManager.keyboard.Changed(Keys.Down))
                {
                    menu.nextItem();
                    move.Play();
                }

                if (screenManager.keyboard.IsDownTime(Keys.Down) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Down, 3 * KEY_PRESSED_TIME);
                    menu.nextItem();
                    move.Play();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Up))
            {
                if (screenManager.keyboard.Changed(Keys.Up))
                {
                    menu.previousItem();
                    move.Play();
                }

                if (screenManager.keyboard.IsDownTime(Keys.Up) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Up, 3 * KEY_PRESSED_TIME);
                    menu.previousItem();
                    move.Play();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
