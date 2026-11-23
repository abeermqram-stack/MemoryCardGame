using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MemoryGameApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace MemoryGameApi.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Player player)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JwtSettings:SecretKey is missing.");
            var issuer = _configuration["JwtSettings:Issuer"] ?? "MemoryGameApi";
            var audience = _configuration["JwtSettings:Audience"] ?? "MemoryGameClient";
            var expiryMinutes = int.TryParse(_configuration["JwtSettings:ExpiryMinutes"], out var mins) ? mins : 120;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, player.PlayerId.ToString()),
                new Claim(ClaimTypes.Name, player.Username),
                new Claim(ClaimTypes.Email, player.Email),
                new Claim(ClaimTypes.Role, player.Role),
                new Claim(JwtRegisteredClaimNames.Sub, player.PlayerId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

