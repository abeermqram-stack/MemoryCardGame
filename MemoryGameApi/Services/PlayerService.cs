using MemoryGameApi.Data;
using MemoryGameApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryGameApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly MemoryGameDbContext _dbContext;

        public PlayerService(MemoryGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Player> GetAllPlayers() =>
            _dbContext.Players
                .Include(p => p.Profile)
                .ToList();

        public Player? GetPlayerById(int id) =>
            _dbContext.Players
                .Include(p => p.Profile)
                .FirstOrDefault(p => p.PlayerId == id);

        public Player? Login(string email, string password) =>
            _dbContext.Players
                .Include(p => p.Profile)
                .FirstOrDefault(p => p.Email == email && p.Password == password);

        public List<Player> SearchPlayersByName(string keyword) =>
            _dbContext.Players
                .Where(p => p.Username.Contains(keyword) || p.Email.Contains(keyword))
                .ToList();

        public Player AddPlayer(Player player)
        {
            if (player.Profile == null)
            {
                player.Profile = new PlayerProfile
                {
                    AvatarEmoji = "👤",
                    PreferredTheme = "Default",
                    HighestScore = 0,
                    TotalGamesPlayed = 0
                };
            }

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            return player;
        }

        public bool UpdatePlayer(Player updatedPlayer)
        {
            var existing = _dbContext.Players.Find(updatedPlayer.PlayerId);
            if (existing == null)
                return false;

            existing.Username = updatedPlayer.Username;
            existing.Email = updatedPlayer.Email;
            existing.Role = updatedPlayer.Role;
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeletePlayer(int id)
        {
            var existing = _dbContext.Players.Find(id);
            if (existing == null)
                return false;

            _dbContext.Players.Remove(existing);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
