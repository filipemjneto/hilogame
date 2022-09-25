using GameServer.GamePlay;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IGame _game;

        public PlayersController(IGame game)
        {
            _game = game;
        }

        // GET: api/<PlayersController>
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _game.GetPlayers().Select(p => (Player)p);
        }

        // GET api/<PlayersController>/5
        [HttpGet("{id}")]
        public Player? Get(string id)
        {
            return (Player?)_game.GetPlayer(id);
        }
    }
}
