using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class PhrasesAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Articles",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title_Date_RobotMapAppId",
                table: "Articles",
                columns: new[] { "Title", "Date", "RobotMapAppId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Title_Date_RobotMapAppId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Articles");
        }
    }
}
