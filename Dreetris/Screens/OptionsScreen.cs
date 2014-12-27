using Dreetris.Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    public class OptionsScreen : MenuScreen
    {
        public OptionsScreen(Game game, ScreenManager screenManager, AssetManager assetManager)
            : base(game, screenManager, assetManager)
        {
            MenuEntryText me1 = new MenuEntryText(game, assetManager, "Volume", doNothing);
            MenuEntryText me2 = new MenuEntryText(game, assetManager, "Ghost", doNothing);
            MenuEntryText me3 = new MenuEntryText(game, assetManager, "Back", back);

            menu.addItem(me1);
            menu.addItem(me2);
            menu.addItem(me3);

            originX = 400;
            originY = 200;
        }

        private void doNothing()
        {

        }

        private void back()
        {
            screenManager.pop();
        }
    }
}
