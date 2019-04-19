using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CompanyId",
                table: "Deliveries",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Companies_CompanyId",
                table: "Deliveries",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Companies_CompanyId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_CompanyId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Deliveries");
        }
    }
}
