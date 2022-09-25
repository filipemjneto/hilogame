using GameServer.GamePlay;
using GameShared;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Hubs
{

    /// <summary>
    /// Class <c>GameHub</c> implements SignalR Hub and contains all the transport logic
    /// </summary>
    public class GameHub : Hub
    {
        private IGame _gaming;

        //Constructor to receive injection
        public GameHub(IGame game)
        {
            _gaming = game;
        }

        /// <summary>
        /// Method <c>PlayGame</c> exposed endpoint for players to submit a bet
        /// </summary>
        /// <param name="connId">Is the internal connection identifier</param>
        /// <param name="playerName">The players chosen name</param>
        /// <param name="chanceNumber">The players bet number</param>
        public async Task PlayGame(string connId, string playerName, int chanceNumber)
        {
            StorePlayer(connId, playerName, chanceNumber);

            await ConfirmBet(playerName, chanceNumber);

            await EvalPlay(connId, playerName, chanceNumber);
        }

        /// <summary>
        /// Method <c>ConfirmBet</c> confirms back to the game client a players guess try.
        /// </summary>
        /// <param name="player">The players chosen name</param>
        /// <param name="number">The players bet number</param>
        private async Task ConfirmBet(string player, int number)
        {
            Console.WriteLine($"{player} tried {number}");

            await Clients.Caller.SendAsync("PlayedGame", number);
        }

        /// <summary>
        /// Method <c>EvalPlay</c> evaluates the players submitted guess number.
        /// </summary>
        /// <param name="conId">The players connection identifier</param>
        /// <param name="name">The players chosen name</param>
        /// <param name="number">The players bet number</param>
        private async Task EvalPlay(string conId, string name, int number)
        {
            HiLo isWinner = _gaming.IsWinner(number);

            await Clients.Caller.SendAsync("GameResult", isWinner);

            if (isWinner == HiLo.Correct)
            {
                IncreasePlayerWins(conId);
                await Clients.Others.SendAsync("GeneralMessage", $"{name} won, game will restart with new secret!");
            }
            else
            {
                await Clients.Others.SendAsync("GeneralMessage", $"{name} took a gamble and missed!");
            }
        }

        /// <summary>
        /// Method <c>IncreasePlayerWins</c> to increament a winning players wins counter.
        /// </summary>
        /// <param name="conId">The players connection identifier</param>
        private void IncreasePlayerWins(string conId)
        {
            _gaming.SetWinner(conId);
        }

        /// <summary>
        /// Method <c>UpdateLimits</c> exposed endpoint for a newly connect game client receive the game limits.
        /// </summary>
        public async Task UpdateLimits()
        {
            var limits = _gaming.GetLimits();

            await Clients.Caller.SendAsync("RefreshLimits", limits.Item1, limits.Item2);
        }

        /// <summary>
        /// Method <c>StorePlayer</c> handles a new player/recurring player presence, and updates the player base data collection.
        /// </summary>
        /// <param name="connectionId">The players connection identifier</param>
        /// <param name="name">The players chosen name</param>
        private void StorePlayer(string connectionId, string name, int bet)
        {
            _gaming.AddPlayer(connectionId, name, bet);
        }

    }
}
