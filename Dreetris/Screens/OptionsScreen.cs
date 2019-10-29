using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dreetris.Screens
{
    public class OptionsScreen : MenuScreen
    {
        private MenuEntrySlider me1;

        public OptionsScreen(GameObjects gameObjects)
            : base(gameObjects)
        {
            me1 = new MenuEntrySlider(gameObjects, "Volume", () => { });
            MenuEntrySelector me2 = new MenuEntrySelector(gameObjects, "Ghost", () => { });
            MenuEntryText me3 = new MenuEntryText(gameObjects, "Back", () => screenManager.Pop().Dispose());

            menu.AddItem(me1);
            menu.AddItem(me2);
            menu.AddItem(me3);

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

            if (menu.GetSelected() == 0)
            {
                if (screenManager.keyboard.downRepeated(Keys.Left, 3 * KEY_PRESSED_TIME / 100))
                {
                    me1.MoveLeft();
                }

                if (screenManager.keyboard.downRepeated(Keys.Right, 3 * KEY_PRESSED_TIME / 100))
                {
                    me1.MoveRight();
                }
            }
        }
    }
}
