
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace Dreetris.Screens
{
    public class Menu
    {
        public delegate void Call();

        public List<MenuEntry> entries = new List<MenuEntry>();

        int position = 0;

        public void addItem(MenuEntry entry)
        {
            entries.Add(entry);
        }

        public void nextItem()
        {
            position = (position + 1) % entries.Count;
        }

        public void previousItem()
        {
            position--;
            if (position < 0)
                position = entries.Count - 1;
        }

        public void callCurrentItem()
        {
            if (entries.Count != 0)
            {
                entries[position].call();
            }
        }

        public float getWidth()
        {
            float max = 0;

            foreach (var e in entries)
            {
                max = Math.Max(max, e.size.X);
            }

            return max;
        }

        public int getSelected()
        {
            return position;
        }
    }
}
