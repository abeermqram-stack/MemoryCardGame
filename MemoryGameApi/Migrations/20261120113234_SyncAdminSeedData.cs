using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class SyncAdminSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 1,
                columns: new[] { "PlayerEmail", "Password", "PlayerName" },
                values: new object[] { "abeer@gmail.com", "1234", "عبير مقرم" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 1,
                columns: new[] { "PlayerEmail", "Password", "PlayerName" },
                values: new object[] { "sara@memorygame.sa", "123456", "سارة العتيبي" });
        }
    }
}
