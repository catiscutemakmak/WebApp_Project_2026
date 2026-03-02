using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "RoomPlayers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RoomPlayers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RankId",
                table: "RoomPlayers",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoleId",
                table: "RoomPlayers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_GameRanks_RankId",
                table: "RoomPlayers",
                column: "RankId",
                principalTable: "GameRanks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers",
                column: "RoleId",
                principalTable: "GameRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_GameRanks_RankId",
                table: "RoomPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers");

            migrationBuilder.DropIndex(
                name: "IX_RoomPlayers_RankId",
                table: "RoomPlayers");

            migrationBuilder.DropIndex(
                name: "IX_RoomPlayers_RoleId",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RoomPlayers");
        }
    }
}
