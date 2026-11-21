using MemoryGameApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryGameApi.Data
{
    public class MemoryGameDbContext : DbContext
    {
        public MemoryGameDbContext(DbContextOptions<MemoryGameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<PlayerAchievement> PlayerAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .ToTable("Players")
                .HasKey(p => p.PlayerId);

            modelBuilder.Entity<Player>()
                .Property(p => p.Username)
                .HasColumnName("PlayerName")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<CardItem>()
                .ToTable("CardItems")
                .HasKey(c => c.CardId);

            modelBuilder.Entity<CardItem>()
                .Property(c => c.Title)
                .HasColumnName("CardTitle")
                .HasMaxLength(80)
                .IsRequired();

            modelBuilder.Entity<Achievement>()
                .Property(a => a.Title)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Profile)
                .WithOne(pr => pr.Player)
                .HasForeignKey<PlayerProfile>(pr => pr.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.GameSessions)
                .WithOne(gs => gs.Player)
                .HasForeignKey(gs => gs.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerAchievement>()
                .HasKey(pa => new { pa.PlayerId, pa.AchievementId });

            modelBuilder.Entity<PlayerAchievement>()
                .HasOne(pa => pa.Player)
                .WithMany(p => p.PlayerAchievements)
                .HasForeignKey(pa => pa.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerAchievement>()
                .HasOne(pa => pa.Achievement)
                .WithMany(a => a.PlayerAchievements)
                .HasForeignKey(pa => pa.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            var seedDate = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<CardItem>().HasData(
                new CardItem { CardId = 1, Title = "صاروخ الفضاء", Icon = "🚀", Category = "الفضاء", AccentColor = "#6366f1", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 2, Title = "كوكب زحل", Icon = "🪐", Category = "الفضاء", AccentColor = "#8b5cf6", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 3, Title = "الجوهرة الماسية", Icon = "💎", Category = "كنوز", AccentColor = "#06b6d4", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 4, Title = "التاج الملكي", Icon = "👑", Category = "كنوز", AccentColor = "#f59e0b", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 5, Title = "الصاعقة الذهبية", Icon = "⚡", Category = "طاقة", AccentColor = "#eab308", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 6, Title = "النجمة المضيئة", Icon = "🌟", Category = "الفضاء", AccentColor = "#ec4899", LevelDifficulty = 1, IsActive = true },
                new CardItem { CardId = 7, Title = "التنين الناري", Icon = "🐉", Category = "أساطير", AccentColor = "#ef4444", LevelDifficulty = 2, IsActive = true },
                new CardItem { CardId = 8, Title = "الكريستال السحري", Icon = "🔮", Category = "أساطير", AccentColor = "#a855f7", LevelDifficulty = 2, IsActive = true },
                new CardItem { CardId = 9, Title = "درع الفرسان", Icon = "🛡️", Category = "مغامرة", AccentColor = "#10b981", LevelDifficulty = 2, IsActive = true },
                new CardItem { CardId = 10, Title = "المفتاح الذهبي", Icon = "🗝️", Category = "كنوز", AccentColor = "#f97316", LevelDifficulty = 2, IsActive = true }
            );

            modelBuilder.Entity<Player>().HasData(
                new Player { PlayerId = 1, Username = "عبير مقرم", Email = "abeer@gmail.com", Password = "1234", Role = "Admin", CreatedAt = seedDate },
                new Player { PlayerId = 2, Username = "آسيا سعيد بن عبيد", Email = "asia@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate },
                new Player { PlayerId = 3, Username = "فاطمة عصام باطويل", Email = "fatima@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate },
                new Player { PlayerId = 4, Username = "حنين عبدالعزيز باوزير", Email = "haneen@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate },
                new Player { PlayerId = 5, Username = "سارة عوض بن جوبح", Email = "sara@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate },
                new Player { PlayerId = 6, Username = "فاطمة بازياد", Email = "fatimab@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate }
            );

            modelBuilder.Entity<PlayerProfile>().HasData(
                new PlayerProfile { ProfileId = 1, PlayerId = 1, AvatarEmoji = "👩‍💻", PreferredTheme = "Cosmic Indigo", HighestScore = 980, TotalGamesPlayed = 5 },
                new PlayerProfile { ProfileId = 2, PlayerId = 2, AvatarEmoji = "🌟", PreferredTheme = "Emerald Glow", HighestScore = 1420, TotalGamesPlayed = 8 },
                new PlayerProfile { ProfileId = 3, PlayerId = 3, AvatarEmoji = "🌸", PreferredTheme = "Royal Gold", HighestScore = 1150, TotalGamesPlayed = 6 },
                new PlayerProfile { ProfileId = 4, PlayerId = 4, AvatarEmoji = "🦋", PreferredTheme = "Soft Rose", HighestScore = 1040, TotalGamesPlayed = 4 },
                new PlayerProfile { ProfileId = 5, PlayerId = 5, AvatarEmoji = "💎", PreferredTheme = "Ocean Pearl", HighestScore = 930, TotalGamesPlayed = 5 },
                new PlayerProfile { ProfileId = 6, PlayerId = 6, AvatarEmoji = "🌙", PreferredTheme = "Warm Mocha", HighestScore = 890, TotalGamesPlayed = 3 }
            );

            modelBuilder.Entity<Achievement>().HasData(
                new Achievement { AchievementId = 1, Title = "ذاكرة فولاذية", Description = "الفوز في المستوى الأول بأقل من 10 محاولات", BadgeIcon = "🧠", BonusPoints = 150 },
                new Achievement { AchievementId = 2, Title = "سريع كالبرق", Description = "إنهاء اللعبة في أقل من 35 ثانية", BadgeIcon = "⚡", BonusPoints = 200 },
                new Achievement { AchievementId = 3, Title = "أسطورة المستوى الثاني", Description = "إتمام المستوى الثاني (10 أزواج) بنجاح", BadgeIcon = "🏆", BonusPoints = 300 }
            );

            modelBuilder.Entity<PlayerAchievement>().HasData(
                new PlayerAchievement { PlayerId = 1, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 2, AchievementId = 2, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 2, AchievementId = 3, UnlockedAt = seedDate, DisplayOrder = 2 },
                new PlayerAchievement { PlayerId = 3, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 3, AchievementId = 2, UnlockedAt = seedDate, DisplayOrder = 2 },
                new PlayerAchievement { PlayerId = 4, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 5, AchievementId = 2, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 6, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 }
            );

            modelBuilder.Entity<GameSession>().HasData(
                new GameSession { SessionId = 1, PlayerId = 2, Level = 2, PairsCount = 10, Attempts = 14, TimeElapsedSeconds = 42, Score = 1420, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 2, PlayerId = 3, Level = 2, PairsCount = 10, Attempts = 16, TimeElapsedSeconds = 48, Score = 1150, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 3, PlayerId = 4, Level = 1, PairsCount = 6, Attempts = 7, TimeElapsedSeconds = 24, Score = 1040, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 4, PlayerId = 1, Level = 1, PairsCount = 6, Attempts = 8, TimeElapsedSeconds = 26, Score = 980, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 5, PlayerId = 5, Level = 1, PairsCount = 6, Attempts = 9, TimeElapsedSeconds = 30, Score = 930, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 6, PlayerId = 6, Level = 1, PairsCount = 6, Attempts = 10, TimeElapsedSeconds = 33, Score = 890, IsCompleted = true, PlayedAt = seedDate }
            );
        }
    }
}
