using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class IndexFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticleCategoryId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_Date_RobotMapAppId",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategoryId_Date_RobotMapAppId",
                table: "Articles",
                columns: new[] { "ArticleCategoryId", "Date", "RobotMapAppId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticleCategoryId_Date_RobotMapAppId",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategoryId",
                table: "Articles",
                column: "ArticleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Date_RobotMapAppId",
                table: "Articles",
                columns: new[] { "Date", "RobotMapAppId" },
                unique: true);
        }
    }
}
