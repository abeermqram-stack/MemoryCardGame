using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthFieldsToPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Players",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 1,
                columns: new[] { "Password", "Role" },
                values: new object[] { "123456", "Admin" });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 2,
                columns: new[] { "Password", "Role" },
                values: new object[] { "123456", "Player" });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: 3,
                columns: new[] { "Password", "Role" },
                values: new object[] { "123456", "Player" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Players");
        }
    }
}
