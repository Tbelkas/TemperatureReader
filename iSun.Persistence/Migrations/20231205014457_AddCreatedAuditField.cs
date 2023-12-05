using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iSun.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAuditField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_temperatureReadings",
                table: "temperatureReadings");

            migrationBuilder.RenameTable(
                name: "temperatureReadings",
                newName: "TemperatureReadings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TemperatureReadings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemperatureReadings",
                table: "TemperatureReadings",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TemperatureReadings",
                table: "TemperatureReadings");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TemperatureReadings");

            migrationBuilder.RenameTable(
                name: "TemperatureReadings",
                newName: "temperatureReadings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_temperatureReadings",
                table: "temperatureReadings",
                column: "Id");
        }
    }
}
