using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris
{
    public class SingleRandom
    {
        private static SingleRandom instance = null;
        private Random _random;

        public static Random random
        {
            get
            {
                if (instance == null)
                {
                    instance = new SingleRandom();
                }
                return instance._random;
            }
        }

        private SingleRandom()
        {
            _random = new Random();
        }

    }
}