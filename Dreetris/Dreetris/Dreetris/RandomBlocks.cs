using System;

namespace Dreetris
{
    class RandomBlocks
    {
        Random random = new Random();
        Tetrimino.Type[] blocks;
        int currentPos = 0;

        public RandomBlocks()
        {
            Randomize();
        }

        public void InitBag()
        {
            blocks = new Tetrimino.Type[7];
            blocks[0] = Tetrimino.Type.I;
            blocks[1] = Tetrimino.Type.J;
            blocks[2] = Tetrimino.Type.L;
            blocks[3] = Tetrimino.Type.O;
            blocks[4] = Tetrimino.Type.S;
            blocks[5] = Tetrimino.Type.T;
            blocks[6] = Tetrimino.Type.Z;
        }

        /// <summary>
        /// Returns a block at position n in the queue.
        /// <param name="n">position in the queue</param>
        /// </summary>
        public Tetrimino.Type GetBlock(int n)
        {
            if (blocks.Length <= n)
                return Tetrimino.Type.None;
            else
            {
                return blocks[n];
            }
        }

        /// <summary>
        /// Returns the block that is scheduled to be used next and randomize.
        /// </summary>
        public Tetrimino.Type GetCurrentBlock()
        {
            Tetrimino.Type block;
            block = blocks[currentPos];
            currentPos++;
            if (currentPos >= blocks.Length)
            {
                Randomize();
            }

            return block;
        }

        /// <summary>
        /// Deletes the first block, shifts the others up and creates a new one in the queue.
        /// </summary>
        public void Randomize()
        {
            InitBag();
            Shuffle(blocks);
            /*
            System.Console.WriteLine("current bag:");
            foreach(var bl in blocks)
            {
                System.Console.WriteLine("cur: {0}", bl);
            }
            */
            currentPos = 0;
        }

        private Tetrimino.Type GetRandomType()
        {
            Array values = Enum.GetValues(typeof(Tetrimino.Type));
            values.GetValue(random.Next(values.Length));
            Tetrimino.Type random_type = Tetrimino.Type.None;

            // Not very well implemented. Need to exclude None in a better manner.
            while (random_type == Tetrimino.Type.None)
                random_type = (Tetrimino.Type)values.GetValue(random.Next(values.Length));

            return random_type;
        }

        /// <summary>
        /// Knuth / Fisher-Yates shuffle
        /// </summary>        
        private void Shuffle(Tetrimino.Type[] array)
        {
            Random random = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1);
                var temp = array[i];
                array[i] = array[n];
                array[n] = temp;
            }
        }
    }
}
