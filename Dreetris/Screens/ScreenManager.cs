using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dreetris.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        #region fields and properties

        private List<Screen> screens = new List<Screen>();
        DKeyboard _keyboard = new DKeyboard();

        public DKeyboard keyboard { get { return _keyboard; } }

        #endregion

        #region constructors

        public ScreenManager(Game game)
            : base(game)
        {
        }

        #endregion

        #region public methods

        public void Push(Screen screen)
        {
            Console.WriteLine(string.Format("Push: {0}", screens.Count));
            if (screens.Count != 0)
                screens[screens.Count - 1].Deactivate();
            screens.Add(screen);
            screens[screens.Count - 1].Activate();
        }

        public Screen Peek()
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

        public Screen Pop()
        {
            Console.WriteLine(string.Format("Pop: {0}", screens.Count));

            if (screens.Count != 0)
            {
                Screen screen = screens[screens.Count - 1];
                screens.RemoveAt(screens.Count - 1);

                screen.Deactivate();
                if (screens.Count != 0)
                    screens[screens.Count - 1].Activate();

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
