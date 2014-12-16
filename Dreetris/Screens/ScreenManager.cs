using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    /// <summary>
    /// This Manages various Screens. Only the Screen at the top of the Stack will be active.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        private List<Screen> screens = new List<Screen>();

        DKeyboard _keyboard = new DKeyboard();
        public DKeyboard keyboard { get { return _keyboard; } }

        public ScreenManager(Game game)
            : base(game)
        { }

        public void push(Screen screen)
        {
            Console.WriteLine(string.Format("Push: {0}", screens.Count));
            if (screens.Count != 0)
                screens.Last().deactivate();
            screens.Add(screen);
            screens.Last().activate();
        }

        public Screen peek()
        {
            if (screens.Count != 0)
            {
                Screen screen = screens.Last();
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
                Screen screen = screens.Last();
                screens.RemoveAt(screens.Count - 1);

                screen.deactivate();
                if (screens.Count != 0)
                    screens.Last().activate();

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
            screens.Last().Update(gameTime);

            base.Update(gameTime);
        }
    }
}
