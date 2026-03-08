using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class Fixmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "RoomPlayers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoomChats",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");


            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RoomId",
                table: "Notifications",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Rooms_RoomId",
                table: "Notifications",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Rooms_RoomId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomChats_AspNetUsers_UserId",
                table: "RoomChats");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RoomId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RoomChats",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

        }
    }
}
