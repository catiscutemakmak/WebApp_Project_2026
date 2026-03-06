using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class updateROle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleName",
                value: "OffLane");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleName",
                value: "MidLane");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleName",
                value: "Carry");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleName",
                value: "Roam");

            migrationBuilder.InsertData(
                table: "GameRoles",
                columns: new[] { "Id", "GameId", "RoleName" },
                values: new object[,]
                {
                    { 18, 5, "GoldLane" },
                    { 19, 5, "ExpLane" },
                    { 20, 5, "MidLane" },
                    { 21, 5, "MarkMan" },
                    { 22, 5, "Support" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleName",
                value: "Top Lane");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleName",
                value: "Mid Lane");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleName",
                value: "ADC");

            migrationBuilder.UpdateData(
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleName",
                value: "Support");
        }
    }
}
