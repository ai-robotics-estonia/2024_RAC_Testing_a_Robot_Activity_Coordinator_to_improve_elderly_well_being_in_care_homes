using App.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class WebLinkDefaultName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LangStr>(
                name: "WebLinkDisplayName",
                table: "WebLinks",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");

            migrationBuilder.AddColumn<string>(
                name: "WebLinkName",
                table: "WebLinks",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebLinkDisplayName",
                table: "WebLinks");

            migrationBuilder.DropColumn(
                name: "WebLinkName",
                table: "WebLinks");
        }
    }
}
