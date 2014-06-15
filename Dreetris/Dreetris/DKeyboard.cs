using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dreetris
{
    public class DKeyboard
    {
        Dictionary<Keys, bool> keys_down = new Dictionary<Keys, bool>();
        Dictionary<Keys, double> key_times = new Dictionary<Keys, double>();

        Dictionary<Keys, bool> last_keys_down = new Dictionary<Keys, bool>();
        int[] keys;

        public DKeyboard()
        {
            keys = Enum.GetValues(typeof(Keys)) as int[];
            foreach (int k in keys)
            {
                keys_down.Add((Keys)k, false);
                key_times.Add((Keys)k, 0);
            }
        }

        public bool changed(Keys key)
        {
            return !(last_keys_down[key] == keys_down[key]);
        }

        public bool is_down(Keys key)
        {
            return keys_down[key];
        }

        public double is_down_time(Keys key)
        {
            return key_times[key];
        }

        public void reset_timer(Keys key, double time)
        {
            key_times[key] -= time;
            if (key_times[key] < 0)
                key_times[key] = 0;
        }

        public void process(GameTime gameTime)
        {
            KeyboardState current_state = Keyboard.GetState();

            foreach (Keys key in keys)            
            {
                last_keys_down[key] = keys_down[key];
                if (current_state.IsKeyUp((Keys)key))
                {
                    keys_down[key] = false;
                    key_times[key] = 0;
                }
                else if (current_state.IsKeyDown((Keys)key))
                {
                    keys_down[key] = true;
                    key_times[key] += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }
    }
}
