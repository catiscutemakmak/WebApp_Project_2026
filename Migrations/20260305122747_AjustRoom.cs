using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class AjustRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId2",
                table: "RoomPlayers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoomId2",
                table: "RoomPlayers",
                column: "RoomId2");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId2",
                table: "RoomPlayers",
                column: "RoomId2",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId2",
                table: "RoomPlayers");

            migrationBuilder.DropIndex(
                name: "IX_RoomPlayers_RoomId2",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "RoomId2",
                table: "RoomPlayers");
        }
    }
}
