using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetApi1.Migrations
{
    public partial class CreatingCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
