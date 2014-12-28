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
        Dictionary<Keys, bool> keysDown = new Dictionary<Keys, bool>();
        Dictionary<Keys, bool> lastKeysDown = new Dictionary<Keys, bool>();        
        Dictionary<Keys, double> keyTimes = new Dictionary<Keys, double>();
        Dictionary<Keys, bool> lockedKeys = new Dictionary<Keys, bool>();

        int[] keys;

        public DKeyboard()
        {
            keys = Enum.GetValues(typeof(Keys)) as int[];
            foreach (int k in keys)
            {
                keysDown.Add((Keys)k, false);
                lastKeysDown.Add((Keys)k, false);
                lockedKeys.Add((Keys)k, false);
                keyTimes.Add((Keys)k, 0);
            }
        }

        /// <summary>
        /// Locks a key so that it has to be released once in order to be consider down once more.
        /// </summary>
        /// <param name="key">key to lock</param>
        public void LockKey(Keys key)
        {
            lockedKeys[key] = true;
            keysDown[key] = false;
        }

        /// <summary>
        /// Check if the status of a key changed since the last call of Process
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Changed(Keys key)
        {
            return !(lastKeysDown[key] == keysDown[key]);
        }

        /// <summary>
        /// Check if key is pressed right now.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsDown(Keys key)
        {
            return keysDown[key];
        }

        /// <summary>
        /// Check since how long key is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>duration of the keypress</returns>
        public double IsDownTime(Keys key)
        {
            return keyTimes[key];
        }

        /// <summary>
        /// Changes the times of the keypress. It substracts the time and sets the value to
        /// zero if the time becomes negative.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        public void ResetTimer(Keys key, double time)
        {
            keyTimes[key] -= time;
            if (keyTimes[key] < 0)
                keyTimes[key] = 0;
        }

        public bool downRepeated(Keys key, double time)
        {
            if (IsDown(key))
            {
                if (Changed(key))
                {
                    return true;
                }
                else if (IsDownTime(key) > time)
                {
                    ResetTimer(key, time);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the keyboard state.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Process(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (Keys key in keys)            
            {
                lastKeysDown[key] = keysDown[key];
                if (currentState.IsKeyUp((Keys)key))
                {
                    lockedKeys[key] = false;
                    keysDown[key] = false;
                    keyTimes[key] = 0;
                }
                else if (currentState.IsKeyDown((Keys)key))
                {
                    if (!lockedKeys[key])
                    {
                        keysDown[key] = true;
                        keyTimes[key] += gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                }
            }
        }
    }
}
