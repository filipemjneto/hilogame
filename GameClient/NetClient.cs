using GameShared;
using Microsoft.AspNetCore.SignalR.Client;

namespace GameClient
{
    /// <summary>
    /// Class <c>NetClient</c> implements a Hub Client, with connection logic and binding of the events generate from the server Hub
    /// </summary>
    /// <remaks>
    /// Implements <c>IDisposable</c> allowing for proper release management
    /// </remaks>
    public class NetClient : IDisposable
    {
        #region Internal Properties
        /// <value>asdfasdf </value>
        private HubConnection _connection;
        private bool disposedValue;
        private static readonly short _port = 5185;
        private static readonly string _host = "localhost";
        private static readonly string _hubPath = "gameHub";

        #endregion

        #region Constructor and Startup

        /// <summary>
        /// <c>NetClient</c> private constructor in line with a factory pattern, and controlled initialization.
        /// </summary>
        private NetClient()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://{_host}:{_port}/{_hubPath}")
                .Build();
        }

        /// <summary>
        /// Method <c>AssignBindings</c> assigns the delegate functions to each event handler, to allow processing of server messaging.
        /// </summary>
        private NetClient AssignBindings(Action<string> onMessageReceived, Action<int> onPlayed, Action<HiLo> handleResult, Action<int, int> handleLimits)
        {
            _connection.Closed += async (error) =>
            {
                if(error != null)
                {
                    Console.WriteLine($"Error: {error.Message}");
                } 
                else
                {
                    Console.WriteLine("Connection closed");
                }
                
                await _connection.StartAsync();
            };

            /*
            _connection.On<string, int>("ReceiveGameChance", (player, number) => {
                Console.WriteLine($"{player} tried to guess with the number {number}");
            });*/

            _connection.On<HiLo>("GameResult", (isWinner) => { handleResult(isWinner); });

            _connection.On<int>("PlayedGame", (number) => { onPlayed(number); });

            _connection.On<string>("GeneralMessage", (msg) => { onMessageReceived(msg); });

            _connection.On<int, int>("RefreshLimits", (min, max) => { handleLimits(min, max); });

            return this;
        }

        /// <summary>
        /// Method <c>Connect</c> attemps and verifies that a successfull connection was made onto the server Hub.
        /// </summary>
        private NetClient Connect()
        {
            _connection.StartAsync().Wait();

            if (_connection.State == HubConnectionState.Connected)
            {
                Console.WriteLine("Sucessfully connected to Game Server");
            }

            return this;
        }

        #endregion

        /// <summary>
        /// Method <c>SetBet</c> sends a new player number guess to the server.
        /// </summary>
        /// <param name="playerName">The players chosen name</param>
        /// <param name="number">The players bet number</param>
        public async Task SetBet(string playerName, int number)
        {
            await _connection.InvokeAsync("PlayGame", _connection.ConnectionId, playerName, number);
        }

        /// <summary>
        /// Method <c>RefreshLimits</c> calls the server to retrieve the minimum and maximum value.
        /// </summary>
        public async Task RefreshLimits()
        {
            await _connection.InvokeAsync("UpdateLimits");
        }

        /// <summary>
        /// Statis method <c>GetInstance</c> implements a factory style initializar.
        /// Requires delegate functions for the appropriate event binding
        /// </summary>
        /// <param name="onMessageReceived">Function receives a string, and handles a generic message event</param>
        /// <param name="onPlayed">Receives a integer value, on the handling of a confirmation event of the guess (Should be the same value sent)</param>
        /// <param name="handleResult">Function that will handle a guess result, requires a <c>HiLo</c> enum type input</param>
        /// <param name="handleLimits">Function that receives two ints, to define the range, first for minimun, second for the maximum</param>
        /// <returns>A new istance of <c>NetClient</c> with it's events bound and connection established</returns>
        public static NetClient GetInstance(Action<string> onMessageReceived, Action<int> onPlayed, Action<HiLo> handleResult, Action<int, int> handleLimits)
        {
            return new NetClient().AssignBindings(onMessageReceived, onPlayed, handleResult, handleLimits).Connect();
        }

        #region Disposable Implementation

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Task tStop = _connection.StopAsync();
                    tStop.Wait();
                    
                    Task tDispose = _connection.DisposeAsync().AsTask();
                    tDispose.Wait();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NetClient()
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
