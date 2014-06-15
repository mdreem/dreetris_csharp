using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris
{
    public class Gamedata
    {
        // See: http://tetris.wikia.com/wiki/Tetris_Worlds
        public static double get_falling_speed(int level)
        {
            return Math.Pow((0.8 - (((double)level - 1.0) * 0.007)), ((double)level - 1)) * 1000;
        }

        public static int getLevel(int rows)
        {
            return ((rows / 10) + 1);
        }
    }
}
