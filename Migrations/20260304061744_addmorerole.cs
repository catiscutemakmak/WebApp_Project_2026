using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class addmorerole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankDivision",
                table: "UserGames",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RankLastUpdated",
                table: "UserGames",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RankTier",
                table: "UserGames",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "GameRanks",
                columns: new[] { "Id", "GameId", "RankImageUrl", "RankName" },
                values: new object[,]
                {
                    { 9, 5, "/images/ranks/mlbb/Iron.webp", "Warrior" },
                    { 10, 5, "/images/ranks/mlbb/Elite.webp", "Elite" },
                    { 11, 5, "/images/ranks/mlbb/Master.webp", "Master" },
                    { 12, 5, "/images/ranks/mlbb/Grandmaster.webp", "Grandmaster" },
                    { 13, 5, "/images/ranks/mlbb/Epic.webp", "Epic" },
                    { 14, 5, "/images/ranks/mlbb/Legend.webp", "Legend" },
                    { 15, 5, "/images/ranks/mlbb/Mythic.webp", "Mythic" },
                    { 16, 3, "/images/ranks/Ow2/Bronze.webp", "Bronze" },
                    { 17, 3, "/images/ranks/Ow2/Silver.webp", "Silver" },
                    { 18, 3, "/images/ranks/Ow2/Gold.webp", "Gold" },
                    { 19, 3, "/images/ranks/Ow2/Platinum.webp", "Platinum" },
                    { 20, 3, "/images/ranks/Ow2/Diamond.webp", "Diamond" },
                    { 21, 3, "/images/ranks/Ow2/Master.webp", "Master" },
                    { 22, 3, "/images/ranks/Ow2/Grandmaster.webp", "Grandmaster" },
                    { 23, 4, "/images/ranks/LoL/Unranked.webp", "Unranked" },
                    { 24, 4, "/images/ranks/LoL/Iron.webp", "Iron" },
                    { 25, 4, "/images/ranks/LoL/Bronze.webp", "Bronze" },
                    { 26, 4, "/images/ranks/LoL/Silver.webp", "Silver" },
                    { 27, 4, "/images/ranks/LoL/Gold.webp", "Gold" },
                    { 28, 4, "/images/ranks/LoL/Platinum.webp", "Platinum" },
                    { 29, 4, "/images/ranks/LoL/Emerald.webp", "Emerald" },
                    { 30, 4, "/images/ranks/LoL/Diamond.webp", "Diamond" },
                    { 31, 4, "/images/ranks/LoL/Master.webp", "Master" },
                    { 32, 4, "/images/ranks/LoL/Grandmaster.webp", "Grandmaster" },
                    { 33, 4, "/images/ranks/LoL/Challenger.webp", "Challenger" },
                    { 34, 9, "/images/ranks/Pubg/Bronze.webp", "Bronze" },
                    { 35, 9, "/images/ranks/Pubg/Silver.webp", "Silver" },
                    { 36, 9, "/images/ranks/Pubg/Gold.webp", "Gold" },
                    { 37, 9, "/images/ranks/Pubg/Platinum.webp", "Platinum" },
                    { 38, 9, "/images/ranks/Pubg/Crown.webp", "Crown" },
                    { 39, 9, "/images/ranks/Pubg/Ace.webp", "Ace" },
                    { 40, 9, "/images/ranks/Pubg/AceMaster.webp", "AceMaster" },
                    { 41, 9, "/images/ranks/Pubg/AceDominator.webp", "AceDominator" },
                    { 42, 9, "/images/ranks/Pubg/Conqueror.webp", "Conqueror" }
                });

            migrationBuilder.InsertData(
                table: "GameRoles",
                columns: new[] { "Id", "GameId", "RoleName" },
                values: new object[,]
                {
                    { 15, 3, "Tank" },
                    { 16, 3, "Damage" },
                    { 17, 3, "Support" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DropColumn(
                name: "RankDivision",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "RankLastUpdated",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "RankTier",
                table: "UserGames");
        }
    }
}
