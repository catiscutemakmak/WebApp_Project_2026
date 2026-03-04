using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class deletepassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserProfiles");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_GameRoles_RoleId",
                table: "RoomPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");

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
    }
}
