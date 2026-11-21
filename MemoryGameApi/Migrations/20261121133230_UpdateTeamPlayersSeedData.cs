using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeamPlayersSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 2,
                columns: new[] { "Attempts", "Level", "PairsCount", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[] { 16, 2, 10, 3, 1150, 48 });

            migrationBuilder.UpdateData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 3,
                columns: new[] { "Attempts", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[] { 7, 4, 1040, 24 });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "SessionId", "Attempts", "IsCompleted", "Level", "PairsCount", "PlayedAt", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[] { 4, 8, true, 1, 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 980, 26 });

            migrationBuilder.InsertData(
                table: "PlayerAchievements",
                columns: new[] { "AchievementId", "PlayerId", "DisplayOrder", "UnlockedAt" },
                values: new object[] { 2, 3, 2, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "PlayerProfiles",
                keyColumn: "ProfileId",
                keyValue: 3,
                columns: new[] { "AvatarEmoji", "HighestScore", "TotalGamesPlayed" },
                values: new object[] { "🌸", 1150, 6 });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 2,
                columns: new[] { "PlayerEmail", "PlayerName" },
                values: new object[] { "asia@memorygame.sa", "آسيا سعيد بن عبيد" });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 3,
                columns: new[] { "PlayerEmail", "PlayerName" },
                values: new object[] { "fatima@memorygame.sa", "فاطمة عصام باطويل" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "CreatedAt", "PlayerEmail", "Password", "Role", "PlayerName" },
                values: new object[,]
                {
                    { 4, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "haneen@memorygame.sa", "123456", "Player", "حنين عبدالعزيز باوزير" },
                    { 5, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "sara@memorygame.sa", "123456", "Player", "سارة عوض بن جوبح" },
                    { 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "fatimab@memorygame.sa", "123456", "Player", "فاطمة بازياد" }
                });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "SessionId", "Attempts", "IsCompleted", "Level", "PairsCount", "PlayedAt", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[,]
                {
                    { 5, 9, true, 1, 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 5, 930, 30 },
                    { 6, 10, true, 1, 6, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 6, 890, 33 }
                });

            migrationBuilder.InsertData(
                table: "PlayerAchievements",
                columns: new[] { "AchievementId", "PlayerId", "DisplayOrder", "UnlockedAt" },
                values: new object[,]
                {
                    { 1, 4, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 5, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 6, 1, new DateTime(2026, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PlayerProfiles",
                columns: new[] { "ProfileId", "AvatarEmoji", "HighestScore", "PlayerId", "PreferredTheme", "TotalGamesPlayed" },
                values: new object[,]
                {
                    { 4, "🦋", 1040, 4, "Soft Rose", 4 },
                    { 5, "💎", 930, 5, "Ocean Pearl", 5 },
                    { 6, "🌙", 890, 6, "Warm Mocha", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PlayerAchievements",
                keyColumns: new[] { "AchievementId", "PlayerId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "PlayerAchievements",
                keyColumns: new[] { "AchievementId", "PlayerId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "PlayerAchievements",
                keyColumns: new[] { "AchievementId", "PlayerId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "PlayerAchievements",
                keyColumns: new[] { "AchievementId", "PlayerId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "PlayerProfiles",
                keyColumn: "ProfileId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PlayerProfiles",
                keyColumn: "ProfileId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlayerProfiles",
                keyColumn: "ProfileId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 2,
                columns: new[] { "Attempts", "Level", "PairsCount", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[] { 8, 1, 6, 1, 980, 26 });

            migrationBuilder.UpdateData(
                table: "GameSessions",
                keyColumn: "SessionId",
                keyValue: 3,
                columns: new[] { "Attempts", "PlayerId", "Score", "TimeElapsedSeconds" },
                values: new object[] { 10, 3, 890, 33 });

            migrationBuilder.UpdateData(
                table: "PlayerProfiles",
                keyColumn: "ProfileId",
                keyValue: 3,
                columns: new[] { "AvatarEmoji", "HighestScore", "TotalGamesPlayed" },
                values: new object[] { "🦊", 890, 3 });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 2,
                columns: new[] { "PlayerEmail", "PlayerName" },
                values: new object[] { "noura@memorygame.sa", "نورة القحطاني" });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 3,
                columns: new[] { "PlayerEmail", "PlayerName" },
                values: new object[] { "reem@memorygame.sa", "ريم الحربي" });
        }
    }
}
