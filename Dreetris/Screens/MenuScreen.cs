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
        protected const int KEY_PRESSED_TIME = 150;

        protected SpriteFont font;
        protected Menu menu;
        protected SpriteBatch spriteBatch;
        protected Sprite pointerLeft;
        protected Sprite pointerRight;

        protected SoundEffect move;

        //position of the menu
        protected float originX = 400;
        protected float originY = 120;

        protected AssetManager assetManager;
        protected Texture2D blank;

        public MenuScreen(Game game, ScreenManager screenManager, AssetManager assetManager)
            : base(game, screenManager)
        {
            this.assetManager = assetManager;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = assetManager.getFont("Font");

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            pointerLeft = assetManager.getSprite("button");
            pointerRight = assetManager.getSprite("button");

            pointerLeft.centerCoordinates();
            pointerRight.centerCoordinates();

            menu = new Menu();

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

            //clear screen
            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            Color col = new Color(180, 160, 190);
            drawPointer();
            drawItems(col);

            spriteBatch.End();
        }

        private void drawPointer()
        {
            if (menu.items.Count == 0) return;

            float menuWidth = menu.getWidth(font);
            float itemHeight = font.MeasureString(menu.items[menu.getSelected()]).Y;

            //TODO: genauer, falls Höhen unterschiedlich
            pointerLeft.position = new Vector2((originX - menuWidth * 1.05f), originY + itemHeight * menu.getSelected() + itemHeight / 2);
            pointerRight.position = new Vector2((originX + menuWidth * 1.05f), originY + itemHeight * menu.getSelected() + itemHeight / 2);

            pointerLeft.draw(spriteBatch);
            pointerRight.draw(spriteBatch);
        }

        private void drawItems(Color col)
        {
            var tmpOrigin = originY;
            foreach (var s in menu.items)
            {
                var measurement = font.MeasureString(s);
                spriteBatch.DrawString(font, s, new Vector2(originX - measurement.X / 2 + 2, tmpOrigin + 2), Color.Black); //Shadow
                spriteBatch.DrawString(font, s, new Vector2(originX - measurement.X / 2, tmpOrigin), col);

                tmpOrigin += measurement.Y;
            }
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

            if (screenManager.keyboard.IsDown(Keys.Enter))
            {
                if (screenManager.keyboard.Changed(Keys.Enter))
                {
                    menu.callCurrentItem();
                }

                if (screenManager.keyboard.IsDownTime(Keys.Enter) > 3 * KEY_PRESSED_TIME)
                {
                    screenManager.keyboard.ResetTimer(Keys.Enter, 3 * KEY_PRESSED_TIME);
                    menu.callCurrentItem();
                }
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
