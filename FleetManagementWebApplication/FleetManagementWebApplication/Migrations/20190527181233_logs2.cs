using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class logs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "FuelLogs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "FuelLogs");
        }
    }
}
