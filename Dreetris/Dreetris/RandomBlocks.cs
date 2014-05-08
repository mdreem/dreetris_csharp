using System;

namespace Dreetris
{
    class RandomBlocks
    {
        Random random = new Random();
        Tetrimino.Type[] blocks;
        int current_pos = 0;

        public RandomBlocks(int size)
        {
            blocks = new Tetrimino.Type[size];

            for (int i = 0; i < size; i++)
            {
                blocks[i] = get_random_type();
            }
        }

        /// <summary>
        /// Returns a block at position n in the queue.
        /// <param name="n">position in the queue</param>
        /// </summary>
        public Tetrimino.Type get_block(int n)
        {
            if (blocks.Length <= n)
                return Tetrimino.Type.None;
            else
                return blocks[(n + current_pos) % blocks.Length];
        }

        /// <summary>
        /// Returns the block that is scheduled to be used next and randomize.
        /// </summary>
        public Tetrimino.Type get_current_block()
        {
            Tetrimino.Type cur = get_block(0);
            randomize();
            return cur;
        }

        /// <summary>
        /// Deletes the first block, shifts the others up and creates a new one in the queue.
        /// </summary>
        public void randomize()
        {
            current_pos += 1;
            current_pos = current_pos % blocks.Length;

            blocks[current_pos] = get_random_type();
        }

        private Tetrimino.Type get_random_type()
        {
            Array values = Enum.GetValues(typeof(Tetrimino.Type));
            values.GetValue(random.Next(values.Length));
            Tetrimino.Type random_type = Tetrimino.Type.None;

            // Not very well implemented. Need to exclude None in a better manner.
            while (random_type == Tetrimino.Type.None)
                random_type = (Tetrimino.Type)values.GetValue(random.Next(values.Length));

            return random_type;
        }
    }
}
