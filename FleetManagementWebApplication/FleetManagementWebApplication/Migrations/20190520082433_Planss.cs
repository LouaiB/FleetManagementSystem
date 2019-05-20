using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class Planss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Plan",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plan_CompanyId",
                table: "Plan",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_Companies_CompanyId",
                table: "Plan",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plan_Companies_CompanyId",
                table: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Plan_CompanyId",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Plan");
        }
    }
}
