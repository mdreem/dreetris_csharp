
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace Dreetris.Screens
{
    public class Menu
    {
        public delegate void Call();

        List<string> _items = new List<string>();
        List<Call> calls = new List<Call>();

        int position = 0;

        public List<string> items { get { return _items;  } }

        public void addItem(string name, Call call)
        {
            _items.Add(name);
            calls.Add(call);
        }

        public void nextItem()
        {
            position = (position + 1) % _items.Count;
        }

        public void previousItem()
        {
            position--;
            if (position < 0)
                position = _items.Count - 1;
        }

        public void callCurrentItem()
        {
            if (calls.Count != 0)
            {
                calls[position].Invoke();
            }
        }

        public float getWidth(SpriteFont font)
        {
            float max = 0;

            foreach(var s in items)
            {
                max = Math.Max(max, font.MeasureString(s).X);
            }
            return max;
        }

        public int getSelected()
        {
            return position;
        }
    }
}
