using System.Security.Claims;
using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IPlayerService playerService, IJwtTokenService jwtTokenService)
        {
            _playerService = playerService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { code = "MISSING_CREDENTIALS", message = "الرجاء إدخال البريد الإلكتروني وكلمة المرور" });

            var existingByEmail = _playerService.GetPlayerByEmail(request.Email);
            if (existingByEmail == null)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "لا يوجد حساب مسجّل بهذا البريد الإلكتروني، يجب عليك إنشاء حساب جديد أولاً." });

            if (existingByEmail.Password != request.Password)
                return Unauthorized(new { code = "INVALID_PASSWORD", message = "كلمة المرور غير صحيحة" });

            var token = _jwtTokenService.GenerateToken(existingByEmail);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Player = existingByEmail
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Email) || string.IsNullOrWhiteSpace(player.Username) || string.IsNullOrWhiteSpace(player.Password))
                return BadRequest(new { code = "MISSING_FIELDS", message = "الرجاء تعبئة الاسم والبريد الإلكتروني وكلمة المرور" });

            if (_playerService.GetPlayerByEmail(player.Email) != null)
                return Conflict(new { code = "EMAIL_EXISTS", message = "هذا البريد الإلكتروني مسجّل مسبقاً" });

            var created = _playerService.AddPlayer(player);
            var token = _jwtTokenService.GenerateToken(created);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Player = created
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var playerId))
                return Unauthorized(new { code = "INVALID_TOKEN", message = "رمز المصادقة غير صالح" });

            var player = _playerService.GetPlayerById(playerId);
            if (player == null)
                return NotFound(new { code = "ACCOUNT_NOT_FOUND", message = "تم حذف هذا الحساب من قاعدة البيانات" });

            return Ok(player);
        }
    }
}

