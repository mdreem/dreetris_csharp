namespace Dreetris
{
    class Score
    {
        int currentScore;
        int currentLevel = 1;

        public int score
        {
            get { return currentScore; }
        }

        public int level
        {
            get { return currentLevel; }
        }

        public Score()
        {

        }

        public int GetScore()
        {
            return currentScore;
        }

        /*
        Single	100 x level
        Double	300 x level
        Triple	500 x level
        Tetris	800 x level; difficult
         */
        public void RowsDeleted(int n)
        {
            int multiplicator = 0;

            switch (n)
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
            currentScore += currentLevel * multiplicator;
        }

        public void NextLevel()
        {
            SetLevel(currentLevel + 1);
        }

        public void SetLevel(int n)
        {
            currentLevel = n;
        }
    }
}
