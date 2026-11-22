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
                .Include(p => p.GameSessions)
                .Include(p => p.PlayerAchievements)
                    .ThenInclude(pa => pa.Achievement)
                .ToList();

        public Player? GetPlayerById(int id) =>
            _dbContext.Players
                .Include(p => p.Profile)
                .Include(p => p.GameSessions)
                .Include(p => p.PlayerAchievements)
                    .ThenInclude(pa => pa.Achievement)
                .FirstOrDefault(p => p.PlayerId == id);

        public Player? GetPlayerByEmail(string email)
        {
            var normalized = (email ?? string.Empty).Trim().ToLower();
            return _dbContext.Players
                .Include(p => p.Profile)
                .Include(p => p.GameSessions)
                .Include(p => p.PlayerAchievements)
                    .ThenInclude(pa => pa.Achievement)
                .FirstOrDefault(p => p.Email.ToLower() == normalized);
        }

        public Player? Login(string email, string password)
        {
            var player = GetPlayerByEmail(email);
            if (player == null || player.Password != password)
                return null;

            return player;
        }

        public List<Player> SearchPlayersByName(string keyword) =>
            _dbContext.Players
                .Include(p => p.Profile)
                .Where(p => p.Username.Contains(keyword) || p.Email.Contains(keyword))
                .ToList();

        public Player AddPlayer(Player player)
        {
            player.PlayerId = 0;
            player.Username = (player.Username ?? string.Empty).Trim();
            player.Email = (player.Email ?? string.Empty).Trim();
            player.Role = "Player";
            player.CreatedAt = DateTime.UtcNow;
            player.GameSessions = new List<GameSession>();
            player.PlayerAchievements = new List<PlayerAchievement>();
            player.Profile = new PlayerProfile
            {
                AvatarEmoji = string.IsNullOrWhiteSpace(player.Profile?.AvatarEmoji) ? "👤" : player.Profile.AvatarEmoji.Trim(),
                PreferredTheme = string.IsNullOrWhiteSpace(player.Profile?.PreferredTheme) ? "Default" : player.Profile.PreferredTheme.Trim(),
                HighestScore = 0,
                TotalGamesPlayed = 0
            };

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            return player;
        }

        public bool UpdatePlayer(Player updatedPlayer)
        {
            var existing = _dbContext.Players.Find(updatedPlayer.PlayerId);
            if (existing == null)
                return false;

            existing.Username = (updatedPlayer.Username ?? existing.Username).Trim();
            existing.Email = (updatedPlayer.Email ?? existing.Email).Trim();
            if (!string.IsNullOrWhiteSpace(updatedPlayer.Password))
            {
                existing.Password = updatedPlayer.Password;
            }
            if (!string.IsNullOrWhiteSpace(updatedPlayer.Role) && !string.Equals(existing.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                existing.Role = updatedPlayer.Role.Trim();
            }
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeletePlayer(int id)
        {
            var existing = _dbContext.Players
                .Include(p => p.Profile)
                .Include(p => p.GameSessions)
                .Include(p => p.PlayerAchievements)
                .FirstOrDefault(p => p.PlayerId == id);

            if (existing == null)
                return false;

            if (string.Equals(existing.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                return false;

            if (existing.GameSessions.Any())
                _dbContext.GameSessions.RemoveRange(existing.GameSessions);

            if (existing.PlayerAchievements.Any())
                _dbContext.PlayerAchievements.RemoveRange(existing.PlayerAchievements);

            if (existing.Profile != null)
                _dbContext.PlayerProfiles.Remove(existing.Profile);

            _dbContext.Players.Remove(existing);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
