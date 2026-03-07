using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class updateRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameRanks",
                columns: new[] { "Id", "GameId", "RankImageUrl", "RankName" },
                values: new object[,]
                {
                    { 43, 2, "/images/ranks/val/Ascendant.webp", "Ascendant" },
                    { 44, 6, "/images/ranks/RoV/Bronze.webp", "Bronze" },
                    { 45, 6, "/images/ranks/RoV/Silver.webp", "Silver" },
                    { 46, 6, "/images/ranks/RoV/Gold.jpg", "Gold" },
                    { 47, 6, "/images/ranks/RoV/Platinum.jpg", "Platinum" },
                    { 48, 6, "/images/ranks/RoV/Diamond.webp", "Diamond" },
                    { 49, 6, "/images/ranks/RoV/Commander.webp", "Commander" },
                    { 50, 6, "/images/ranks/RoV/Conqueror.webp", "Conqueror" },
                    { 51, 1, "/images/ranks/CS2/Silver.png", "Silver" },
                    { 52, 1, "/images/ranks/CS2/Gold.png", "Gold Nova" },
                    { 53, 1, "/images/ranks/CS2/MasterGuardian.png", "Master Guardian" },
                    { 54, 1, "/images/ranks/CS2/Distinguished.png", "Distinguished" },
                    { 55, 1, "/images/ranks/CS2/LegendaryEagle.png", "Legendary Eagle" },
                    { 56, 1, "/images/ranks/CS2/SupremeMaster.png", "Supreme" },
                    { 57, 1, "/images/ranks/CS2/Global.png", "Global Elite" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 57);
        }
    }
}
