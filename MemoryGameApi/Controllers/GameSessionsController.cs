using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoryGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionsController : ControllerBase
    {
        private readonly IGameSessionService _sessionService;

        public GameSessionsController(IGameSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult GetAllSessions()
        {
            return Ok(_sessionService.GetAllSessions());
        }

        // [FromQuery] Parameter Binding
        [HttpGet("paged")]
        public IActionResult GetPagedSessions([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_sessionService.GetPagedSessions(pageIndex, pageSize));
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetSessionById([FromRoute] int id)
        {
            var session = _sessionService.GetSessionById(id);
            if (session == null)
                return NotFound("No Session was found");

            return Ok(session);
        }

        // Add Operation
        [HttpPost]
        public IActionResult AddSession([FromBody] GameSession session)
        {
            var created = _sessionService.AddSession(session);
            return Ok(created);
        }

        // Update Operation
        [HttpPut("{id:int:min(1)}")]
        public IActionResult UpdateSession([FromRoute] int id, [FromBody] GameSession session)
        {
            session.SessionId = id;
            var updated = _sessionService.UpdateSession(session);
            if (!updated)
                return NotFound("No Session was found");

            return Ok(session);
        }

        // Delete Operation
        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteSession([FromRoute] int id)
        {
            var deleted = _sessionService.DeleteSession(id);
            if (!deleted)
                return NotFound("No Session was found");

            return Ok(new { message = "Session Deleted Successfully" });
        }
    }
}
