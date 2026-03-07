using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class addusergame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_UserProfileId_GameId",
                table: "UserGames",
                columns: new[] { "UserProfileId", "GameId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGames_UserProfileId_GameId",
                table: "UserGames");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames",
                column: "UserProfileId");
        }
    }
}
