using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class AddRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/val/Ascendant.webp", "Ascendant" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/val/Immortal.webp", "Immortal" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 2, "/images/ranks/val/Radiant.webp", "Radiant" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Warrior.webp", "Warrior" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Elite.webp", "Elite" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Epic.webp", "Epic" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Legend.webp", "Legend" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 5, "/images/ranks/mlbb/Mythic.webp", "Mythic" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Diamond.webp", "Diamond" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 3, "/images/ranks/Ow2/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Unranked.webp", "Unranked" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Iron.webp", "Iron" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Emerald.webp", "Emerald" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Diamond.webp", "Diamond" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 4, "/images/ranks/LoL/Challenger.webp", "Challenger" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Crown.webp", "Crown" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Ace.webp", "Ace" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/AceMaster.webp", "AceMaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/AceDominator.webp", "AceDominator" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 9, "/images/ranks/Pubg/Conqueror.webp", "Conqueror" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/val/Immortal.webp", "Immortal" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/val/Radiant.webp", "Radiant" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 5, "/images/ranks/mlbb/Warrior.webp", "Warrior" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Elite.webp", "Elite" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Epic.webp", "Epic" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Legend.webp", "Legend" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/mlbb/Mythic.webp", "Mythic" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 3, "/images/ranks/Ow2/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Diamond.webp", "Diamond" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Ow2/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 4, "/images/ranks/LoL/Unranked.webp", "Unranked" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Iron.webp", "Iron" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Emerald.webp", "Emerald" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Diamond.webp", "Diamond" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Master.webp", "Master" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Grandmaster.webp", "Grandmaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/LoL/Challenger.webp", "Challenger" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 9, "/images/ranks/Pubg/Bronze.webp", "Bronze" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Silver.webp", "Silver" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Gold.webp", "Gold" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Platinum.webp", "Platinum" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Crown.webp", "Crown" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Ace.webp", "Ace" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/AceMaster.webp", "AceMaster" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/AceDominator.webp", "AceDominator" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "RankImageUrl", "RankName" },
                values: new object[] { "/images/ranks/Pubg/Conqueror.webp", "Conqueror" });

            migrationBuilder.UpdateData(
                table: "GameRanks",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "GameId", "RankImageUrl", "RankName" },
                values: new object[] { 2, "/images/ranks/val/Ascendant.webp", "Ascendant" });
        }
    }
}
