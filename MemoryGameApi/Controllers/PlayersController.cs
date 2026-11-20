using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoryGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            return Ok(_playerService.GetAllPlayers());
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetPlayerById([FromRoute] int id)
        {
            var player = _playerService.GetPlayerById(id);
            if (player == null)
                return NotFound("No Player was found");

            return Ok(player);
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string email, [FromQuery] string password)
        {
            var player = _playerService.Login(email, password);
            if (player == null)
                return NotFound("Invalid email or password");

            return Ok(player);
        }

        [HttpGet("search/{keyword:alpha}")]
        public IActionResult SearchPlayers([FromRoute] string keyword)
        {
            return Ok(_playerService.SearchPlayersByName(keyword));
        }

        [HttpPost]
        public IActionResult AddPlayer([FromBody] Player player)
        {
            var created = _playerService.AddPlayer(player);
            return Ok(created);
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult UpdatePlayer([FromRoute] int id, [FromBody] Player player)
        {
            player.PlayerId = id;
            var updated = _playerService.UpdatePlayer(player);
            if (!updated)
                return NotFound("No Player was found");

            return Ok(player);
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeletePlayer([FromRoute] int id)
        {
            var deleted = _playerService.DeletePlayer(id);
            if (!deleted)
                return NotFound("No Player was found");

            return Ok(new { message = "Player Deleted Successfully" });
        }
    }
}
