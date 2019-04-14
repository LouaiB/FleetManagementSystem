using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class Maintenance1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlanId",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlanId",
                table: "Vehicles",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Plan_PlanId",
                table: "Vehicles",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Plan_PlanId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_PlanId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vehicles");
        }
    }
}
