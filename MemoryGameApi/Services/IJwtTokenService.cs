using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Player player);
    }
}

