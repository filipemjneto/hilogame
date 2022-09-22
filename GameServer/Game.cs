namespace GameServer
{
    public class Game
    {
        private readonly int _min;
        private readonly int _max;
        private int _secretNumber;

        private Game(int min, int max)
        {
            _min = min;
            _max = max;
            GenerateSecret();
        }

        private void GenerateSecret()
        {
            Random r = new();

            _secretNumber = r.Next(_min, _max);

            Console.WriteLine($"Secret Number is: {_secretNumber}");
        }

        public bool IsWinner(int number)
        {
            if(_secretNumber == number)
            {
                GenerateSecret();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Tuple<int, int> GetLimits()
        {
            return Tuple.Create(_min, _max);
        }

        public static Game GetGame(int min, int max)
        {
            return new Game(min, max);
        }
    }
}
