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

            // =========================================================
            // 1. Configuring Non-Relational Properties (Fluent API)
            // =========================================================
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

            // =========================================================
            // 2. One-to-One Relationship (Player <-> PlayerProfile)
            // =========================================================
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Profile)
                .WithOne(pr => pr.Player)
                .HasForeignKey<PlayerProfile>(pr => pr.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // 3. One-to-Many Relationship (Player -> GameSessions)
            // =========================================================
            modelBuilder.Entity<Player>()
                .HasMany(p => p.GameSessions)
                .WithOne(gs => gs.Player)
                .HasForeignKey(gs => gs.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // 4. Many-to-Many Relationship (Player <-> Achievement)
            //    via Linking Table (PlayerAchievement)
            // =========================================================
            // Configure Composite Primary Key for Linking Table
            modelBuilder.Entity<PlayerAchievement>()
                .HasKey(pa => new { pa.PlayerId, pa.AchievementId });

            // Configure 1:N Relationship (Player -> PlayerAchievement)
            modelBuilder.Entity<PlayerAchievement>()
                .HasOne(pa => pa.Player)
                .WithMany(p => p.PlayerAchievements)
                .HasForeignKey(pa => pa.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure 1:N Relationship (Achievement -> PlayerAchievement)
            modelBuilder.Entity<PlayerAchievement>()
                .HasOne(pa => pa.Achievement)
                .WithMany(a => a.PlayerAchievements)
                .HasForeignKey(pa => pa.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // 5. Seed Initial Data (Cards, Players, Profiles, Sessions, Achievements)
            // =========================================================
            var seedDate = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc);

            // Seed 10 Card Pairs (First 6 for Level 1, All 10 for Level 2)
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

            // Seed Players (with Password & Role for Authentication)
            modelBuilder.Entity<Player>().HasData(
                new Player { PlayerId = 1, Username = "عبير مقرم", Email = "abeer@gmail.com", Password = "1234", Role = "Admin", CreatedAt = seedDate },
                new Player { PlayerId = 2, Username = "نورة القحطاني", Email = "noura@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate },
                new Player { PlayerId = 3, Username = "ريم الحربي", Email = "reem@memorygame.sa", Password = "123456", Role = "Player", CreatedAt = seedDate }
            );

            // Seed PlayerProfiles (1:1 with Players)
            modelBuilder.Entity<PlayerProfile>().HasData(
                new PlayerProfile { ProfileId = 1, PlayerId = 1, AvatarEmoji = "👩‍💻", PreferredTheme = "Cosmic Indigo", HighestScore = 980, TotalGamesPlayed = 5 },
                new PlayerProfile { ProfileId = 2, PlayerId = 2, AvatarEmoji = "🌟", PreferredTheme = "Emerald Glow", HighestScore = 1420, TotalGamesPlayed = 8 },
                new PlayerProfile { ProfileId = 3, PlayerId = 3, AvatarEmoji = "🦊", PreferredTheme = "Royal Gold", HighestScore = 890, TotalGamesPlayed = 3 }
            );

            // Seed Achievements
            modelBuilder.Entity<Achievement>().HasData(
                new Achievement { AchievementId = 1, Title = "ذاكرة فولاذية", Description = "الفوز في المستوى الأول بأقل من 10 محاولات", BadgeIcon = "🧠", BonusPoints = 150 },
                new Achievement { AchievementId = 2, Title = "سريع كالبرق", Description = "إنهاء اللعبة في أقل من 35 ثانية", BadgeIcon = "⚡", BonusPoints = 200 },
                new Achievement { AchievementId = 3, Title = "أسطورة المستوى الثاني", Description = "إتمام المستوى الثاني (10 أزواج) بنجاح", BadgeIcon = "🏆", BonusPoints = 300 }
            );

            // Seed PlayerAchievements (N:M Linking Table)
            modelBuilder.Entity<PlayerAchievement>().HasData(
                new PlayerAchievement { PlayerId = 1, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 2, AchievementId = 2, UnlockedAt = seedDate, DisplayOrder = 1 },
                new PlayerAchievement { PlayerId = 2, AchievementId = 3, UnlockedAt = seedDate, DisplayOrder = 2 },
                new PlayerAchievement { PlayerId = 3, AchievementId = 1, UnlockedAt = seedDate, DisplayOrder = 1 }
            );

            // Seed GameSessions (1:N with Players)
            modelBuilder.Entity<GameSession>().HasData(
                new GameSession { SessionId = 1, PlayerId = 2, Level = 2, PairsCount = 10, Attempts = 14, TimeElapsedSeconds = 42, Score = 1420, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 2, PlayerId = 1, Level = 1, PairsCount = 6, Attempts = 8, TimeElapsedSeconds = 26, Score = 980, IsCompleted = true, PlayedAt = seedDate },
                new GameSession { SessionId = 3, PlayerId = 3, Level = 1, PairsCount = 6, Attempts = 10, TimeElapsedSeconds = 33, Score = 890, IsCompleted = true, PlayedAt = seedDate }
            );
        }
    }
}

