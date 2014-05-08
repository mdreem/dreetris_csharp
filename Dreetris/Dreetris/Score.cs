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
        int base_score = 100;

        public Score()
        {
            
        }

        public int get_score()
        {
            return current_score;
        }

        public void rows_deleted(int n)
        {
            int multiplicator = 1;

            switch(n)
            {
                case 1:
                    multiplicator = 1;
                    break;
                case 2:
                    multiplicator = 2;
                    break;
                case 3:
                    multiplicator = 4;
                    break;
                case 4:
                    multiplicator = 8;
                    break;
                default:
                    break;
            }

            current_score += multiplicator * n * base_score;
        }

        public void next_level()
        {
            set_level(current_level + 1);
        }

        public void set_level(int n)
        {
            current_level = n;
            base_score = 100 * n;
        }

    }
}
