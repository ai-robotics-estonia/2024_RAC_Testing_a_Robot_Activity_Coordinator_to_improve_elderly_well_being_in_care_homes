using System;
using App.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GreetingPhraseCategoryId",
                table: "GreetingPhrases",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "GreetingPhraseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CategoryDisplayName = table.Column<LangStr>(type: "jsonb", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreetingPhraseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreetingPhraseCategories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebLinkCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CategoryDisplayName = table.Column<LangStr>(type: "jsonb", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebLinkCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebLinkCategories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    IsIframe = table.Column<bool>(type: "boolean", nullable: false),
                    ZoomFactor = table.Column<float>(type: "real", nullable: true),
                    TextZoom = table.Column<int>(type: "integer", nullable: true),
                    WebLinkCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebLinks_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebLinks_WebLinkCategories_WebLinkCategoryId",
                        column: x => x.WebLinkCategoryId,
                        principalTable: "WebLinkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreetingPhrases_GreetingPhraseCategoryId",
                table: "GreetingPhrases",
                column: "GreetingPhraseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingPhraseCategories_OrganizationId",
                table: "GreetingPhraseCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WebLinkCategories_OrganizationId",
                table: "WebLinkCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WebLinks_OrganizationId",
                table: "WebLinks",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WebLinks_WebLinkCategoryId",
                table: "WebLinks",
                column: "WebLinkCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GreetingPhrases_GreetingPhraseCategories_GreetingPhraseCate~",
                table: "GreetingPhrases",
                column: "GreetingPhraseCategoryId",
                principalTable: "GreetingPhraseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GreetingPhrases_GreetingPhraseCategories_GreetingPhraseCate~",
                table: "GreetingPhrases");

            migrationBuilder.DropTable(
                name: "GreetingPhraseCategories");

            migrationBuilder.DropTable(
                name: "WebLinks");

            migrationBuilder.DropTable(
                name: "WebLinkCategories");

            migrationBuilder.DropIndex(
                name: "IX_GreetingPhrases_GreetingPhraseCategoryId",
                table: "GreetingPhrases");

            migrationBuilder.DropColumn(
                name: "GreetingPhraseCategoryId",
                table: "GreetingPhrases");
        }
    }
}
