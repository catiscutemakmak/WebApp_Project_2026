using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hateekub.Migrations
{
    /// <inheritdoc />
    public partial class AddStartTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Rooms",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Rooms");
        }
    }
}
