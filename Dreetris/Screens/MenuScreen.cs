using System;
using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    public class MenuScreen : Screen
    {
        protected static readonly int KEY_PRESSED_TIME = 150;

        protected SpriteFont font;
        protected Menu menu;
        protected SpriteBatch spriteBatch;
        protected Sprite pointerLeft;
        protected Sprite pointerRight;

        protected SoundEffect move;

        protected float originX = 400;
        protected float originY = 120;

        protected AssetManager assetManager;
        protected Texture2D blank;

        protected float pointerScale = 1.2f;

        public MenuScreen(GameObjects gameObjects)
            : base(gameObjects)
        {
            this.assetManager = gameObjects.AssetManager;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = assetManager.GetFont("Font");

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            pointerLeft = assetManager.GetSprite("button");
            pointerRight = assetManager.GetSprite("button");

            pointerLeft.CenterCoordinates();
            pointerRight.CenterCoordinates();

            menu = new Menu();

            move = assetManager.GetSoundEffect("selection");
        }

        protected void DoNothing()
        {
            Console.WriteLine("doNothing");
        }

        public override void Update(GameTime gameTime)
        {
            screenManager.keyboard.Process(gameTime);
            ProcessKeyboard(gameTime);

            pointerLeft.Update(gameTime);
            pointerRight.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), Color.Black * 0.75f);

            DrawPointer();
            DrawItems();


            spriteBatch.End();
        }

        private void DrawPointer()
        {
            if (menu.entries.Count == 0) return;

            float menuWidth = menu.GetWidth();
            float itemHeight = menu.entries[menu.GetSelected()].Size.Y;

            //TODO: different height. Be more specific
            pointerLeft.position = new Vector2((originX - (menuWidth / 2) * pointerScale),
                originY + itemHeight * menu.GetSelected() + itemHeight / 2);
            pointerRight.position = new Vector2((originX + (menuWidth / 2) * pointerScale),
                originY + itemHeight * menu.GetSelected() + itemHeight / 2);

            pointerLeft.Draw(spriteBatch);
            pointerRight.Draw(spriteBatch);
        }

        private void DrawItems()
        {
            var tmpOrigin = originY;
            foreach (var e in menu.entries)
            {
                float width = e.Size.X;
                float height = e.Size.Y;

                e.Position = new Vector2(originX - width / 2, tmpOrigin);
                e.draw(spriteBatch);

                tmpOrigin += height;
            }
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            if (screenManager.keyboard.downRepeated(Keys.Down, 3 * KEY_PRESSED_TIME))
            {
                menu.NextItem();
                move.Play();
            }

            if (screenManager.keyboard.downRepeated(Keys.Up, 3 * KEY_PRESSED_TIME))
            {
                menu.PreviousItem();
                move.Play();
            }

            if (screenManager.keyboard.downRepeated(Keys.Enter, 3 * KEY_PRESSED_TIME))
            {
                menu.CallCurrentItem();
            }

            if (screenManager.keyboard.IsDown(Keys.Escape))
                Game.Exit();
        }
    }
}
