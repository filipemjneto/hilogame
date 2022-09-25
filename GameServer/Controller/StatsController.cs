using GameServer.GamePlay;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private IGame _game;

        public StatsController(IGame game)
        {
            _game = game;
        }

        // GET api/<StatsController>/GetGamesPlayedCounter
        [HttpGet("GetGamesPlayedCounter")]
        public int GetGamesPlayedCounter()
        {
            return _game.GetPlayedCounter();
        }

        // GET api/<StatsController>/GetWinsCounter
        [HttpGet("GetWinsCounter")]
        public int GetWinsCounter()
        {
            return _game.GetWinsCounter();
        }

        // GET api/<StatsController>/GetPlayersCounter
        [HttpGet("GetPlayersCounter")]
        public int GetPlayersCounter()
        {
            return _game.PlayersRegistered();
        }
    }
}
