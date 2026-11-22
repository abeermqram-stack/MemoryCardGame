using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoryGameApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsToCardItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "CardItems",
                type: "int",
                nullable: false,
                defaultValue: 100);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 1,
                column: "Points",
                value: 100);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 2,
                column: "Points",
                value: 100);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 3,
                column: "Points",
                value: 150);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 4,
                column: "Points",
                value: 150);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 5,
                column: "Points",
                value: 120);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 6,
                column: "Points",
                value: 100);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 7,
                column: "Points",
                value: 200);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 8,
                column: "Points",
                value: 180);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 9,
                column: "Points",
                value: 180);

            migrationBuilder.UpdateData(
                table: "CardItems",
                keyColumn: "CardId",
                keyValue: 10,
                column: "Points",
                value: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "CardItems");
        }
    }
}
