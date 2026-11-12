using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BadgeIcon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BonusPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.AchievementId);
                });

            migrationBuilder.CreateTable(
                name: "CardItems",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    AccentColor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LevelDifficulty = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardItems", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlayerEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PairsCount = table.Column<int>(type: "int", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    TimeElapsedSeconds = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_GameSessions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAchievements",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAchievements", x => new { x.PlayerId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_PlayerAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerAchievements_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProfiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarEmoji = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PreferredTheme = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HighestScore = table.Column<int>(type: "int", nullable: false),
                    TotalGamesPlayed = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "AchievementId", "BadgeIcon", "BonusPoints", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "🧠", 150, "الفوز في المستوى الأول بأقل من 10 محاولات", "ذاكرة فولاذية" },
                    { 2, "⚡", 200, "إنهاء اللعبة في أقل من 35 ثانية", "سريع كالبرق" },
                    { 3, "🏆", 300, "إتمام المستوى الثاني (10 أزواج) بنجاح", "أسطورة المستوى الثاني" }
                });

            migrationBuilder.InsertData(
                table: "CardItems",
                columns: new[] { "CardId", "AccentColor", "Category", "Icon", "IsActive", "LevelDifficulty", "CardTitle" },
                values: new object[,]
                {
                    { 1, "#6366f1", "الفضاء", "🚀", true, 1, "صاروخ الفضاء" },
                    { 2, "#8b5cf6", "الفضاء", "🪐", true, 1, "كوكب زحل" },
                    { 3, "#06b6d4", "كنوز", "💎", true, 1, "الجوهرة الماسية" },
                    { 4, "#f59e0b", "كنوز", "👑", true, 1, "التاج الملكي" },
                    { 5, "#eab308", "طاقة", "⚡", true, 1, "الصاعقة الذهبية" },
                    { 6, "#ec4899", "الفضاء", "🌟", true, 1, "النجمة المضيئة" },
                    { 7, "#ef4444", "أساطير", "🐉", true, 2, "التنين الناري" },
                    { 8, "#a855f7", "أساطير", "🔮", true, 2, "الكريستال السحري" },
                    { 9, "#10b981", "مغامرة", "🛡️", true, 2, "درع الفرسان" },
                    { 10, "#f97316", "كنوز", "🗝️", true, 2, "المفتاح الذهبي" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "CreatedAt", "PlayerEmail", "PlayerName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "sara@memorygame.sa", "سارة العتيبي" },
                    { 2, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "noura@memorygame.sa", "نورة القحطاني" },
                    { 3, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "reem@memorygame.sa", "ريم الحربي" }
                });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "SessionId", "Attempts", "IsCompleted", "Level", "PairsCount", "PlayedAt", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[,]
                {
                    { 1, 14, true, 2, 10, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, 1420, 42 },
                    { 2, 8, true, 1, 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 980, 26 },
                    { 3, 10, true, 1, 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 3, 890, 33 }
                });

            migrationBuilder.InsertData(
                table: "PlayerAchievements",
                columns: new[] { "AchievementId", "PlayerId", "DisplayOrder", "UnlockedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 2, 2, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 3, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PlayerProfiles",
                columns: new[] { "ProfileId", "AvatarEmoji", "HighestScore", "PlayerId", "PreferredTheme", "TotalGamesPlayed" },
                values: new object[,]
                {
                    { 1, "👩‍💻", 980, 1, "Cosmic Indigo", 5 },
                    { 2, "🌟", 1420, 2, "Emerald Glow", 8 },
                    { 3, "🦊", 890, 3, "Royal Gold", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_PlayerId",
                table: "GameSessions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAchievements_AchievementId",
                table: "PlayerAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_PlayerId",
                table: "PlayerProfiles",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardItems");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "PlayerAchievements");

            migrationBuilder.DropTable(
                name: "PlayerProfiles");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
