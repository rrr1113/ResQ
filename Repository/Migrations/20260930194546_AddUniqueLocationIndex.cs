using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueLocationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherSnapshot_Incidents_IncidentId",
                table: "WeatherSnapshot");

            migrationBuilder.DropIndex(
                name: "IX_WeatherSnapshot_IncidentId",
                table: "WeatherSnapshot");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "WeatherSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Address_City_Country",
                table: "Locations",
                columns: new[] { "Address", "City", "Country" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Address_City_Country",
                table: "Locations");

            migrationBuilder.AddColumn<Guid>(
                name: "IncidentId",
                table: "WeatherSnapshot",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherSnapshot_IncidentId",
                table: "WeatherSnapshot",
                column: "IncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherSnapshot_Incidents_IncidentId",
                table: "WeatherSnapshot",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id");
        }
    }
}
