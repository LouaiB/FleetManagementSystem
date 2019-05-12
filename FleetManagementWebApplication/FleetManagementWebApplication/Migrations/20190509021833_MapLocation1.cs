using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class MapLocation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "MapLocations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapLocations_CompanyId",
                table: "MapLocations",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocations_Companies_CompanyId",
                table: "MapLocations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLocations_Companies_CompanyId",
                table: "MapLocations");

            migrationBuilder.DropIndex(
                name: "IX_MapLocations_CompanyId",
                table: "MapLocations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MapLocations");
        }
    }
}
