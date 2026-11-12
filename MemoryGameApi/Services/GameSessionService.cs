using MemoryGameApi.Data;
using MemoryGameApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryGameApi.Services
{
    public class GameSessionService : IGameSessionService
    {
        private readonly MemoryGameDbContext _dbContext;

        public GameSessionService(MemoryGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GameSession> GetAllSessions() =>
            _dbContext.GameSessions
                .Include(gs => gs.Player)
                    .ThenInclude(p => p!.Profile)
                .OrderByDescending(gs => gs.Score)
                .ThenBy(gs => gs.TimeElapsedSeconds)
                .ToList();

        public List<GameSession> GetPagedSessions(int pageIndex, int pageSize, int? level = null)
        {
            var query = _dbContext.GameSessions
                .Include(gs => gs.Player)
                    .ThenInclude(p => p!.Profile)
                .AsQueryable();

            if (level.HasValue)
            {
                query = query.Where(gs => gs.Level == level.Value);
            }

            return query
                .OrderByDescending(gs => gs.Score)
                .ThenBy(gs => gs.TimeElapsedSeconds)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public GameSession? GetSessionById(int id) =>
            _dbContext.GameSessions
                .Include(gs => gs.Player)
                .FirstOrDefault(gs => gs.SessionId == id);

        public GameSession AddSession(GameSession session)
        {
            session.PlayedAt = DateTime.UtcNow;
            session.PairsCount = session.Level == 1 ? 6 : 10;

            // Ensure player exists; if not, attach to Player 1 or create a new Player
            var player = _dbContext.Players
                .Include(p => p.Profile)
                .Include(p => p.PlayerAchievements)
                .FirstOrDefault(p => p.PlayerId == session.PlayerId);

            if (player == null)
            {
                player = _dbContext.Players
                    .Include(p => p.Profile)
                    .Include(p => p.PlayerAchievements)
                    .FirstOrDefault();
                if (player != null)
                {
                    session.PlayerId = player.PlayerId;
                }
            }

            // Prevent EF from trying to insert an existing nested Player object
            session.Player = null;

            _dbContext.GameSessions.Add(session);

            // Update 1:1 PlayerProfile stats & N:M Achievements automatically
            if (player != null)
            {
                if (player.Profile != null)
                {
                    player.Profile.TotalGamesPlayed += 1;
                    if (session.Score > player.Profile.HighestScore)
                    {
                        player.Profile.HighestScore = session.Score;
                    }
                }

                // Auto-unlock Achievement 1: Win Level 1 in <= 10 attempts
                if (session.Level == 1 && session.Attempts <= 10 &&
                    !player.PlayerAchievements.Any(pa => pa.AchievementId == 1))
                {
                    _dbContext.PlayerAchievements.Add(new PlayerAchievement
                    {
                        PlayerId = player.PlayerId,
                        AchievementId = 1,
                        UnlockedAt = DateTime.UtcNow,
                        DisplayOrder = player.PlayerAchievements.Count + 1
                    });
                }

                // Auto-unlock Achievement 2: Win in <= 35 seconds
                if (session.TimeElapsedSeconds <= 35 &&
                    !player.PlayerAchievements.Any(pa => pa.AchievementId == 2))
                {
                    _dbContext.PlayerAchievements.Add(new PlayerAchievement
                    {
                        PlayerId = player.PlayerId,
                        AchievementId = 2,
                        UnlockedAt = DateTime.UtcNow,
                        DisplayOrder = player.PlayerAchievements.Count + 1
                    });
                }

                // Auto-unlock Achievement 3: Complete Level 2 (10 pairs)
                if (session.Level == 2 && session.IsCompleted &&
                    !player.PlayerAchievements.Any(pa => pa.AchievementId == 3))
                {
                    _dbContext.PlayerAchievements.Add(new PlayerAchievement
                    {
                        PlayerId = player.PlayerId,
                        AchievementId = 3,
                        UnlockedAt = DateTime.UtcNow,
                        DisplayOrder = player.PlayerAchievements.Count + 1
                    });
                }
            }

            _dbContext.SaveChanges();

            // Reload navigation property for response
            session.Player = player;
            return session;
        }

        public bool UpdateSession(GameSession updatedSession)
        {
            var existing = _dbContext.GameSessions.Find(updatedSession.SessionId);
            if (existing == null)
                return false;

            existing.Level = updatedSession.Level;
            existing.PairsCount = updatedSession.Level == 1 ? 6 : 10;
            existing.Attempts = updatedSession.Attempts;
            existing.TimeElapsedSeconds = updatedSession.TimeElapsedSeconds;
            existing.Score = updatedSession.Score;
            existing.IsCompleted = updatedSession.IsCompleted;

            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteSession(int id)
        {
            var existing = _dbContext.GameSessions.Find(id);
            if (existing == null)
                return false;

            _dbContext.GameSessions.Remove(existing);
            _dbContext.SaveChanges();
            return true;
        }
    }
}

