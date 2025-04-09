using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddMapFloorIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MapLocations_LocationName_MapId",
                table: "MapLocations");

            migrationBuilder.CreateIndex(
                name: "IX_MapLocations_LocationName_MapFloorId",
                table: "MapLocations",
                columns: new[] { "LocationName", "MapFloorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MapLocations_LocationName_MapFloorId",
                table: "MapLocations");

            migrationBuilder.CreateIndex(
                name: "IX_MapLocations_LocationName_MapId",
                table: "MapLocations",
                columns: new[] { "LocationName", "MapId" },
                unique: true);
        }
    }
}
