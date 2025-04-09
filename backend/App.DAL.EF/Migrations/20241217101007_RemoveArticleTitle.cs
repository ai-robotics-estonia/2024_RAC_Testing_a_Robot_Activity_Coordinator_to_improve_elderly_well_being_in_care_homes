using System;
using App.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArticleTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Title_Date_RobotMapAppId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DisplayTitle",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Articles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleCategoryId",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Date_RobotMapAppId",
                table: "Articles",
                columns: new[] { "Date", "RobotMapAppId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Date_RobotMapAppId",
                table: "Articles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleCategoryId",
                table: "Articles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<LangStr>(
                name: "DisplayTitle",
                table: "Articles",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Articles",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title_Date_RobotMapAppId",
                table: "Articles",
                columns: new[] { "Title", "Date", "RobotMapAppId" },
                unique: true);
        }
    }
}
