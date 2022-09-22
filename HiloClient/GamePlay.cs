namespace HiloClient
{
    internal class GamePlay : IDisposable
    {
        #region Properties and Constructor

        private string _playerName;
        private short _wins = 0;
        private short _games = 0;
        private int _lastBet = 0;
        private int _gameMin = 0;
        private int _gameMax = 0;
        private bool _continueToPlay = true;
        private bool _waitingForReply = true;
        private NetClient _client;
        private bool disposedValue;

        private GamePlay() {
            _playerName = string.Empty;
            _client = NetClient.GetNetClient(this.HandleMessages, this.HandlePlayed, this.HandleGameResult, this.HandleLimits);
        }

        #endregion

        public async Task GameLoop()
        {
            await GetLimits();

            RequestPlayerName();

            while(_continueToPlay)
            {
                await Play();

                while(_waitingForReply) { }
            }

            Console.WriteLine("Press Enter to exit!");
            Console.ReadLine();
        }

        public string GetResults() => $"You played {_games} times, and won {_wins} times!";

        #region Internal user interface methods
        private void RequestPlayerName()
        {
            Console.WriteLine("Please input player name?");

            _playerName = Console.ReadLine() ?? String.Empty;

            if(_playerName == null || _playerName.Trim().Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Invalid player name!");

                RequestPlayerName();
            }
        }

        private async Task Play()
        {
            _waitingForReply = true;
            await _client.SetBet(_playerName, GetNumberGuess());
            _games++;
        }

        private async Task GetLimits()
        {
            await _client.RefreshLimits();
        }

        private int GetNumberGuess()
        {
            Console.WriteLine($"{_playerName} please try to guess a number between {_gameMin} and {_gameMax}");

            if (int.TryParse(Console.ReadLine(), out int guess))
            {
                _lastBet = guess;
                return guess;
            }
            else
            {
                return GetNumberGuess();
            }
        }

        #endregion

        #region Message Handlers

        public void HandleLimits(int min, int max)
        {
            _gameMin = min;
            _gameMax = max;
        }

        public void HandleMessages(string msg)
        {
            Console.WriteLine(msg);
        }

        public void HandlePlayed(int number)
        {
            Console.WriteLine($"You bet with {number}");
        }

        public void HandleGameResult(bool isWinner)
        {
            if (isWinner)
            {
                Console.WriteLine($"Congrats you guessed it right!{_lastBet} was the lucky number!");
                _wins++;
            }
            else
            {
                Console.WriteLine("You didn't win, do you want to try again? (Y/N)");
                string response = Console.ReadLine() ?? "Y";
                _continueToPlay =  response == "Y" || response == "y";
            }

            _waitingForReply = false;
        }

        #endregion

        public static GamePlay GetGamePlay()
        {
            return new GamePlay();
        }

        #region Disposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                _continueToPlay = false;
                _waitingForReply = false;
                _playerName = string.Empty;

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GamePlay()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

#endregion
    }
}
