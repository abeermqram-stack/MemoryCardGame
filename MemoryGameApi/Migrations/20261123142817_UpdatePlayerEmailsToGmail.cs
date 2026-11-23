using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerEmailsToGmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 2,
                column: "PlayerEmail",
                value: "asia@gmail.com");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 3,
                column: "PlayerEmail",
                value: "fatima@gmail.com");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 4,
                column: "PlayerEmail",
                value: "haneen@gmail.com");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 5,
                column: "PlayerEmail",
                value: "sara@gmail.com");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 6,
                column: "PlayerEmail",
                value: "fatimab@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 2,
                column: "PlayerEmail",
                value: "asia@memorygame.sa");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 3,
                column: "PlayerEmail",
                value: "fatima@memorygame.sa");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 4,
                column: "PlayerEmail",
                value: "haneen@memorygame.sa");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 5,
                column: "PlayerEmail",
                value: "sara@memorygame.sa");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 6,
                column: "PlayerEmail",
                value: "fatimab@memorygame.sa");
        }
    }
}
