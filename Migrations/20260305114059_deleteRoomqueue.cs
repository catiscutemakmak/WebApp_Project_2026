using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class deleteRoomqueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomQueues");

            migrationBuilder.AddColumn<bool>(
                name: "IsInQueue",
                table: "RoomPlayers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "RoomPlayers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoomId1",
                table: "RoomPlayers",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId1",
                table: "RoomPlayers",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId1",
                table: "RoomPlayers");

            migrationBuilder.DropIndex(
                name: "IX_RoomPlayers_RoomId1",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "IsInQueue",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "RoomPlayers");

            migrationBuilder.CreateTable(
                name: "RoomQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QueuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomQueues_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomQueues_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomQueues_RoomId",
                table: "RoomQueues",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomQueues_UserId",
                table: "RoomQueues",
                column: "UserId");
        }
    }
}
