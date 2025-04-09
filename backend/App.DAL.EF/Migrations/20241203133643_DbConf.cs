using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class DbConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_RobotMapApps_RobotMapAppId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEvents_RobotMapApps_RobotMapAppId",
                table: "LogEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_RobotMapApps_RobotMapAppId",
                table: "Articles",
                column: "RobotMapAppId",
                principalTable: "RobotMapApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogEvents_RobotMapApps_RobotMapAppId",
                table: "LogEvents",
                column: "RobotMapAppId",
                principalTable: "RobotMapApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_RobotMapApps_RobotMapAppId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEvents_RobotMapApps_RobotMapAppId",
                table: "LogEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_RobotMapApps_RobotMapAppId",
                table: "Articles",
                column: "RobotMapAppId",
                principalTable: "RobotMapApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogEvents_RobotMapApps_RobotMapAppId",
                table: "LogEvents",
                column: "RobotMapAppId",
                principalTable: "RobotMapApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
