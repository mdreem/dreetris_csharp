using Dreetris.Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dreetris.Screens
{
    /// <summary>
    /// This class manages the various Screens. Only the Screen at the top of the Stack will be active.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region fields and properties

        private List<Screen> screens = new List<Screen>();

        DKeyboard _keyboard = new DKeyboard();
        public DKeyboard keyboard { get { return _keyboard; } }
        AssetManager assetManager;

        #endregion

        #region constructors

        public ScreenManager(Game game, AssetManager assetManager)
            : base(game)
        {
            this.assetManager = assetManager;
        }

        #endregion

        #region public methods

        public void push(Screen screen)
        {
            Console.WriteLine(string.Format("Push: {0}", screens.Count));
            if (screens.Count != 0)
                screens[screens.Count - 1].deactivate();
            screens.Add(screen);
            screens[screens.Count - 1].activate();
        }

        public Screen peek()
        {
            if (screens.Count != 0)
            {
                Screen screen = screens[screens.Count - 1];
                return screen;
            }
            else
            {
                return null;
            }
        }

        public Screen pop()
        {
            Console.WriteLine(string.Format("Pop: {0}", screens.Count));

            if (screens.Count != 0)
            {
                Screen screen = screens[screens.Count - 1];
                screens.RemoveAt(screens.Count - 1);

                screen.deactivate();
                if (screens.Count != 0)
                    screens[screens.Count - 1].activate();

                return screen;
            }
            else
            {
                return null;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Screen s in this.screens)
            {
                if (s.visible)
                {
                    s.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            screens[screens.Count - 1].Update(gameTime);

            base.Update(gameTime);
        }

        public void printStack()
        {
            Console.WriteLine("ScreenManager: ");
            foreach (var e in screens)
            {
                Console.WriteLine("{0}", e.ToString());
            }
        }

        #endregion
    }
}
