using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class FixGameIdReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "GameName", "GameType", "MaxPlayers", "MinPlayers" },
                values: new object[,]
                {
                    { 1, "CS2", "FPS", 5, 1 },
                    { 2, "Valorant", "FPS", 5, 1 },
                    { 3, "Overwatch", "FPS", 5, 1 },
                    { 4, "LoL", "MOBA", 5, 1 },
                    { 5, "Mobile Legends", "MOBA", 5, 1 },
                    { 6, "RoV", "MOBA", 5, 1 },
                    { 7, "Among Us", "Party", 15, 4 },
                    { 8, "Peak", "Party", 4, 1 },
                    { 9, "PUBG", "Battle Royale", 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "GameRanks",
                columns: new[] { "Id", "GameId", "RankImageUrl", "RankName" },
                values: new object[,]
                {
                    { 1, 2, "/images/ranks/val/iron.png", "Iron" },
                    { 2, 2, "/images/ranks/val/bronze.png", "Bronze" },
                    { 3, 2, "/images/ranks/val/silver.png", "Silver" },
                    { 4, 2, "/images/ranks/val/gold.png", "Gold" },
                    { 5, 2, "/images/ranks/val/plat.png", "Platinum" },
                    { 6, 2, "/images/ranks/val/diamond.png", "Diamond" },
                    { 7, 2, "/images/ranks/val/immortal.png", "Immortal" },
                    { 8, 2, "/images/ranks/val/radiant.png", "Radiant" }
                });

            migrationBuilder.InsertData(
                table: "GameRoles",
                columns: new[] { "Id", "GameId", "RoleName" },
                values: new object[,]
                {
                    { 1, 2, "Duelist" },
                    { 2, 2, "Controller" },
                    { 3, 2, "Initiator" },
                    { 4, 2, "Sentinel" },
                    { 5, 4, "Top Lane" },
                    { 6, 4, "Jungle" },
                    { 7, 4, "Mid Lane" },
                    { 8, 4, "ADC" },
                    { 9, 4, "Support" },
                    { 10, 6, "Top Lane" },
                    { 11, 6, "Jungle" },
                    { 12, 6, "Mid Lane" },
                    { 13, 6, "ADC" },
                    { 14, 6, "Support" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
