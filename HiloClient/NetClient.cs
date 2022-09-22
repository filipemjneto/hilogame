using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.SignalR.Client;

namespace HiloClient
{
    public class NetClient : IDisposable
    {
        #region Internal Properties

        private HubConnection _connection;
        private bool disposedValue;
        private static readonly short _port = 5185;
        private static readonly string _host = "localhost";
        private static readonly string _hubPath = "gameHub";

        #endregion

        #region Constructor and Startup

        private NetClient()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://{_host}:{_port}/{_hubPath}")
                .Build();
        }

        private NetClient AssignBindings(Action<string> onMessageReceived, Action<int> onPlayed, Action<bool> handleResult, Action<int, int> handleLimits)
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

            _connection.On<bool>("GameResult", (isWinner) => { handleResult(isWinner); });

            _connection.On<int>("PlayedGame", (number) => { onPlayed(number); });

            _connection.On<string>("GeneralMessage", (msg) => { onMessageReceived(msg); });

            _connection.On<int, int>("RefreshLimits", (min, max) => { handleLimits(min, max); });

            return this;
        }

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

        public async Task RefreshLimits()
        {
            await _connection.InvokeAsync("UpdateLimits");
        }

        public static NetClient GetNetClient(Action<string> onMessageReceived, Action<int> onPlayed, Action<bool> handleResult, Action<int, int> handleLimits)
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
