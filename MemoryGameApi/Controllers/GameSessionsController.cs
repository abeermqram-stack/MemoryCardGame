using System.Security.Claims;
using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("paged")]
        public IActionResult GetPagedSessions(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? level = null)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0.");

            return Ok(_sessionService.GetPagedSessions(pageIndex, pageSize, level));
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetSessionById([FromRoute] int id)
        {
            var session = _sessionService.GetSessionById(id);
            if (session == null)
                return NotFound("No Session was found");

            return Ok(session);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSession([FromBody] GameSession session)
        {
            var isAdmin = User.IsInRole("Admin");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!isAdmin && int.TryParse(currentUserId, out var parsedId))
            {
                session.PlayerId = parsedId;
            }

            var created = _sessionService.AddSession(session);
            if (created == null)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "الحساب غير موجود، يرجى إنشاء حساب جديد." });

            return Ok(created);
        }

        [HttpPut("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSession([FromRoute] int id, [FromBody] GameSession session)
        {
            session.SessionId = id;
            var updated = _sessionService.UpdateSession(session);
            if (!updated)
                return NotFound("No Session was found");

            return Ok(session);
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSession([FromRoute] int id)
        {
            var deleted = _sessionService.DeleteSession(id);
            if (!deleted)
                return NotFound("No Session was found");

            return Ok(new { message = "Session Deleted Successfully" });
        }
    }
}
