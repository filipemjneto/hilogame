namespace GameServer.GamePlay
{
    /// <summary>
    /// Class <c>Player</c> exist to have a base structure to where store some player data
    /// </summary>
    public class Player : IPlayer
    {
        private readonly string _connectionId;
        public string Id => _connectionId;
        public string GetId() => _connectionId;

        private readonly string _name;
        public string Name => _name;
        public string GetName() => _name;

        private int _wins = 0;
        public int GamesWon => _wins;

        private int _played;
        public int GamesPlayed => _played;

        private int _bet;
        public void SetBet(int bet) => _bet = bet;
        public int LastBet => _bet;

        public Player(string connectionId, string name)
        {
            _connectionId = connectionId;
            _name = name;
        }

        public void Won() { _wins++; }
        public void Played() { _played++; }
    }

    public interface IPlayer
    {
        void SetBet(int bet);

        void Won();
        void Played();

        string GetId();
        string GetName();
    }
}
