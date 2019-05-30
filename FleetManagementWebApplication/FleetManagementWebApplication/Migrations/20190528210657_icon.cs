using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class icon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Vehicles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Vehicles");
        }
    }
}
