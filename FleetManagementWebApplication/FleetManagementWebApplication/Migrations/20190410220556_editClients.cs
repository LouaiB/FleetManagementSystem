using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class editClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clients",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clients",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12);
        }
    }
}
