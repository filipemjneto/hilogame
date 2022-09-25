using GameServer.GamePlay;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private IGame _game;

        public StatsController(IGame game)
        {
            _game = game;
        }

        [HttpGet]
        public Stats Get()
        {
            return new Stats()
            {
                GamesPlayed = _game.GetPlayedCounter(),
                Wins = _game.GetWinsCounter(),
                Players = _game.PlayersRegistered()
            };
        }
    }

    public class Stats
    {
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Players { get; set; }
    }
}
