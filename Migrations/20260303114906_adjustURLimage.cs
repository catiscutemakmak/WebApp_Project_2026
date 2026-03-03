using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class adjustURLimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 1,
                column: "RankImageUrl",
                value: "/images/ranks/val/Iron.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 2,
                column: "RankImageUrl",
                value: "/images/ranks/val/Bronze.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 3,
                column: "RankImageUrl",
                value: "/images/ranks/val/Silver.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 4,
                column: "RankImageUrl",
                value: "/images/ranks/val/Gold.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 5,
                column: "RankImageUrl",
                value: "/images/ranks/val/Platinum.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 6,
                column: "RankImageUrl",
                value: "/images/ranks/val/Diamond.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 7,
                column: "RankImageUrl",
                value: "/images/ranks/val/Immortal.webp");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 8,
                column: "RankImageUrl",
                value: "/images/ranks/val/Radiant.webp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 1,
                column: "RankImageUrl",
                value: "/images/ranks/val/iron.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 2,
                column: "RankImageUrl",
                value: "/images/ranks/val/bronze.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 3,
                column: "RankImageUrl",
                value: "/images/ranks/val/silver.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 4,
                column: "RankImageUrl",
                value: "/images/ranks/val/gold.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 5,
                column: "RankImageUrl",
                value: "/images/ranks/val/plat.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 6,
                column: "RankImageUrl",
                value: "/images/ranks/val/diamond.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 7,
                column: "RankImageUrl",
                value: "/images/ranks/val/immortal.png");

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 8,
                column: "RankImageUrl",
                value: "/images/ranks/val/radiant.png");
        }
    }
}
