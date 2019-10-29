using System;

namespace Dreetris
{
    public class RandomBlocks
    {
        Tetrimino.Type[] blocks;
        Tetrimino.Type[] nextBlocks;
        int currentPos;

        public RandomBlocks()
        {
            blocks = InitBlocks();
            nextBlocks = InitBlocks();
            Shuffle(nextBlocks);
            Randomize();
        }

        private Tetrimino.Type[] InitBlocks()
        {
            Tetrimino.Type[] array = new Tetrimino.Type[7];
            array[0] = Tetrimino.Type.I;
            array[1] = Tetrimino.Type.J;
            array[2] = Tetrimino.Type.L;
            array[3] = Tetrimino.Type.O;
            array[4] = Tetrimino.Type.S;
            array[5] = Tetrimino.Type.T;
            array[6] = Tetrimino.Type.Z;

            return array;
        }

        public void InitBag()
        {
            blocks = nextBlocks;
            nextBlocks = InitBlocks();
        }

        public Tetrimino.Type GetBlock(int n)
        {
            int pos = currentPos + n;

            if (pos >= blocks.Length && pos < blocks.Length + nextBlocks.Length)
            {
                return nextBlocks[pos - blocks.Length];
            }
            else if (pos < blocks.Length)
            {
                return blocks[pos];
            }
            else
            {
                //TODO: Exception?
                return Tetrimino.Type.None;
            }
        }

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

        public void Randomize()
        {
            InitBag();
            Shuffle(nextBlocks);
            currentPos = 0;
        }

        private Tetrimino.Type GetRandomType()
        {
            Random random = new Random();
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
