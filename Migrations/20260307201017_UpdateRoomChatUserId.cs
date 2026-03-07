using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomChatUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomChats_UserProfiles_UserId",
                table: "RoomChats");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoomChats",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomChats_AspNetUsers_UserId",
                table: "RoomChats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomChats_AspNetUsers_UserId",
                table: "RoomChats");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RoomChats",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomChats_UserProfiles_UserId",
                table: "RoomChats",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
