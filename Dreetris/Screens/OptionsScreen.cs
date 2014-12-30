using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    public class OptionsScreen : MenuScreen
    {
        private MenuEntrySlider me1;

        public OptionsScreen(GameObjects gameObjects)
            : base(gameObjects)
        {
            me1 = new MenuEntrySlider(gameObjects, "Volume", () => { });
            MenuEntryText me2 = new MenuEntryText(gameObjects, "Ghost", () => { });
            MenuEntryText me3 = new MenuEntryText(gameObjects, "Back", () => screenManager.pop().Dispose());

            menu.addItem(me1);
            menu.addItem(me2);
            menu.addItem(me3);

            originX = 400;
            originY = 200;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (menu.getSelected() == 0)
            {
                if (screenManager.keyboard.downRepeated(Keys.Left, 3 * KEY_PRESSED_TIME / 100))
                {
                    me1.moveLeft();
                }

                if (screenManager.keyboard.downRepeated(Keys.Right, 3 * KEY_PRESSED_TIME / 100))
                {
                    me1.moveRight();
                }
            }
        }
    }
}
