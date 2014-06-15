using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris
{
    class Score
    {
        int current_score;
        int current_level = 1;

        public Score()
        {
            
        }

        public int get_score()
        {
            return current_score;
        }

        /*
        Single	100 x level
        Double	300 x level
        Triple	500 x level
        Tetris	800 x level; difficult
         */
        public void rows_deleted(int n)
        {
            int multiplicator = 1;

            switch(n)
            {
                case 1:
                    multiplicator = 100;
                    break;
                case 2:
                    multiplicator = 300;
                    break;
                case 3:
                    multiplicator = 500;
                    break;
                case 4:
                    multiplicator = 800;
                    break;
                default:
                    break;
            }
            current_score += current_level * multiplicator;
        }

        public void next_level()
        {
            set_level(current_level + 1);
        }

        public void set_level(int n)
        {
            current_level = n;
        }
    }
}
