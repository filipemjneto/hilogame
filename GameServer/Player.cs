namespace GameServer
{
    /// <summary>
    /// Class <c>Player</c> exist to have a base structure to where store some player data
    /// </summary>
    public class Player
    {
        public readonly string ConnectionId;
        public readonly string Name;

        private int _wins = 0;
        private int _played = 0;

        public int Bet = 0;

        public Player(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

        public void Won() { _wins++; }
        public void Played() { _played++; }

        public int GetWins => _wins;
        public int GetGamesPlayed => _played;
    }
}
