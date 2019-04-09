using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetApi1.Migrations
{
    public partial class CreatingForeign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyId",
                table: "Drivers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Companies_CompanyId",
                table: "Drivers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Companies_CompanyId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CompanyId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Drivers");
        }
    }
}
