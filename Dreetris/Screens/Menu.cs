using System;
using System.Collections.Generic;

namespace Dreetris.Screens
{
    public class Menu
    {
        public delegate void Call();

        public List<MenuEntry> entries = new List<MenuEntry>();

        int position;

        public void AddItem(MenuEntry entry)
        {
            entries.Add(entry);
        }

        public void NextItem()
        {
            position = (position + 1) % entries.Count;
        }

        public void PreviousItem()
        {
            position--;
            if (position < 0)
                position = entries.Count - 1;
        }

        public void CallCurrentItem()
        {
            if (entries.Count != 0)
            {
                entries[position].call();
            }
        }

        public float GetWidth()
        {
            float max = 0;

            foreach (var e in entries)
            {
                max = Math.Max(max, e.Size.X);
            }

            return max;
        }

        public int GetSelected()
        {
            return position;
        }
    }
}
