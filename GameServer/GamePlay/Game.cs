using System.Numerics;

namespace GameServer.GamePlay
{
    public class Game : IGame
    {
        private readonly int _min;
        private readonly int _max;
        private int _secretNumber;
        private int _totalGamesPlayed;
        private int _totalWins;

        private IList<IPlayer> _players;

        private Game(int min, int max)
        {
            _min = min;
            _max = max;
            GenerateSecret();
            _players = new List<IPlayer>();
        }

        private void GenerateSecret()
        {
            Random r = new();

            _secretNumber = r.Next(_min, _max);

            Console.WriteLine($"Secret Number is: {_secretNumber}");
        }

        public bool IsWinner(int number)
        {
            if (_secretNumber == number)
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

        public IEnumerable<IPlayer> GetPlayers() => _players.AsEnumerable();

        public void AddPlayer(string id, string name, int bet)
        {
            if (!_players.Any(p => p.GetId() == id))
            {
                Player p = new(id, name);

                p.SetBet(bet);
                p.Played();

                _players.Add(p);
            } else
            {
                SetPlayerLastBet(bet, id);
            }

            GamesPlayed();
        }

        private void SetPlayerLastBet(int bet, string id)
        {
            try
            {
                IPlayer? player = _players.SingleOrDefault(p => p.GetId() == id);

                if (player != null)
                {
                    player.SetBet(bet);
                    player.Played();

                    _players[_players.IndexOf(player)] = player;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }

        public void SetWinner(string id)
        {
            try
            {
                IPlayer? player = _players.SingleOrDefault(p => p.GetId() == id);

                if (player != null)
                {
                    player.Won();

                    _players[_players.IndexOf(player)] = player;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.ToString()}");
            }

            _totalWins++;
        }

        private void GamesPlayed()
        {
            Interlocked.Increment(ref _totalGamesPlayed);
        }
        public int GetPlayedCounter() => _totalGamesPlayed;
        public int GetWinsCounter() => _totalWins;
        public int PlayersRegistered() => _players.Count;
    }

    public interface IGame
    {
        bool IsWinner(int number);
        Tuple<int, int> GetLimits();
        IEnumerable<IPlayer> GetPlayers();
        void AddPlayer(string id, string name, int bet);
        void SetWinner(string id);
        int GetPlayedCounter();
        int GetWinsCounter();
        int PlayersRegistered();
    }
}
