using System;
using App.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddMapFloor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MapFloorId",
                table: "MapLocations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapFloors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FloorName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FloorDisplayName = table.Column<LangStr>(type: "jsonb", nullable: false),
                    MapId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapFloors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapFloors_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapLocations_MapFloorId",
                table: "MapLocations",
                column: "MapFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_MapFloors_MapId",
                table: "MapFloors",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations",
                column: "MapFloorId",
                principalTable: "MapFloors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLocations_MapFloors_MapFloorId",
                table: "MapLocations");

            migrationBuilder.DropTable(
                name: "MapFloors");

            migrationBuilder.DropIndex(
                name: "IX_MapLocations_MapFloorId",
                table: "MapLocations");

            migrationBuilder.DropColumn(
                name: "MapFloorId",
                table: "MapLocations");
        }
    }
}
