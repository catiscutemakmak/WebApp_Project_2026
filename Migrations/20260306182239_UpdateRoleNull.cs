using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoomPlayers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoomPlayers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers",
                column: "RoleId",
                principalTable: "GameRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
