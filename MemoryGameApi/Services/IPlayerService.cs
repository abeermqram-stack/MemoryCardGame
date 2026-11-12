using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface IPlayerService
    {
        List<Player> GetAllPlayers();
        Player? GetPlayerById(int id);
        Player? Login(string email, string password);
        List<Player> SearchPlayersByName(string keyword);
        Player AddPlayer(Player player);
        bool UpdatePlayer(Player updatedPlayer);
        bool DeletePlayer(int id);
    }
}
