using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class WebLinkDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BuiltInZoomControls",
                table: "WebLinks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayZoomControls",
                table: "WebLinks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LayoutAlgorithm",
                table: "WebLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "LoadWithOverviewMode",
                table: "WebLinks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseWideViewPort",
                table: "WebLinks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuiltInZoomControls",
                table: "WebLinks");

            migrationBuilder.DropColumn(
                name: "DisplayZoomControls",
                table: "WebLinks");

            migrationBuilder.DropColumn(
                name: "LayoutAlgorithm",
                table: "WebLinks");

            migrationBuilder.DropColumn(
                name: "LoadWithOverviewMode",
                table: "WebLinks");

            migrationBuilder.DropColumn(
                name: "UseWideViewPort",
                table: "WebLinks");
        }
    }
}
