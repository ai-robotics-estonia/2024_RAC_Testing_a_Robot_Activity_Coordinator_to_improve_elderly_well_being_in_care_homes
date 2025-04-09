using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddMapFloorCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations",
                column: "MapFloorId",
                principalTable: "MapFloors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations",
                column: "MapFloorId",
                principalTable: "MapFloors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
