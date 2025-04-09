using System;
using App.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class GreetingPhrase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GreetingPhrases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Phrase = table.Column<LangStr>(type: "jsonb", nullable: false),
                    RobotMapAppId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreetingPhrases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreetingPhrases_RobotMapApps_RobotMapAppId",
                        column: x => x.RobotMapAppId,
                        principalTable: "RobotMapApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreetingPhrases_RobotMapAppId",
                table: "GreetingPhrases",
                column: "RobotMapAppId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreetingPhrases");
        }
    }
}
