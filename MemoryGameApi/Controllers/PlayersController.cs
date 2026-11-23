using System.Security.Claims;
using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllPlayers()
        {
            return Ok(_playerService.GetAllPlayers());
        }

        [HttpGet("{id:int:min(1)}")]
        [Authorize]
        public IActionResult GetPlayerById([FromRoute] int id)
        {
            var isAdmin = User.IsInRole("Admin");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!isAdmin && currentUserId != id.ToString())
                return Forbid();

            var player = _playerService.GetPlayerById(id);
            if (player == null)
                return NotFound("No Player was found");

            return Ok(player);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string email, [FromQuery] string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { code = "MISSING_CREDENTIALS", message = "الرجاء إدخال البريد الإلكتروني وكلمة المرور" });

            var existingByEmail = _playerService.GetPlayerByEmail(email);
            if (existingByEmail == null)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "لا يوجد حساب مسجّل بهذا البريد الإلكتروني، يجب عليك إنشاء حساب جديد أولاً." });

            if (existingByEmail.Password != password)
                return Unauthorized(new { code = "INVALID_PASSWORD", message = "كلمة المرور غير صحيحة" });

            return Ok(existingByEmail);
        }

        [HttpGet("search/{keyword:alpha}")]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchPlayers([FromRoute] string keyword)
        {
            return Ok(_playerService.SearchPlayersByName(keyword));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddPlayer([FromBody] Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Email) || string.IsNullOrWhiteSpace(player.Username) || string.IsNullOrWhiteSpace(player.Password))
                return BadRequest(new { code = "MISSING_FIELDS", message = "الرجاء تعبئة الاسم والبريد الإلكتروني وكلمة المرور" });

            if (_playerService.GetPlayerByEmail(player.Email) != null)
                return Conflict(new { code = "EMAIL_EXISTS", message = "هذا البريد الإلكتروني مسجّل مسبقاً" });

            var created = _playerService.AddPlayer(player);
            return Ok(created);
        }

        [HttpPut("{id:int:min(1)}")]
        [Authorize]
        public IActionResult UpdatePlayer([FromRoute] int id, [FromBody] Player player)
        {
            var isAdmin = User.IsInRole("Admin");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!isAdmin && currentUserId != id.ToString())
                return Forbid();

            var byEmail = _playerService.GetPlayerByEmail(player.Email);
            if (byEmail != null && byEmail.PlayerId != id)
                return Conflict(new { code = "EMAIL_EXISTS", message = "هذا البريد الإلكتروني مستخدم لحساب آخر" });

            player.PlayerId = id;
            var updated = _playerService.UpdatePlayer(player);
            if (!updated)
                return NotFound("No Player was found");

            return Ok(_playerService.GetPlayerById(id));
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePlayer([FromRoute] int id)
        {
            var existing = _playerService.GetPlayerById(id);
            if (existing == null)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "لم يتم العثور على الحساب" });

            if (string.Equals(existing.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { code = "CANNOT_DELETE_ADMIN", message = "لا يمكن حذف حساب المشرف" });

            var deleted = _playerService.DeletePlayer(id);
            if (!deleted)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "لم يتم العثور على الحساب" });

            return Ok(new { message = "تم حذف حساب اللاعب وجميع بياناته نهائياً من قاعدة البيانات" });
        }
    }
}
