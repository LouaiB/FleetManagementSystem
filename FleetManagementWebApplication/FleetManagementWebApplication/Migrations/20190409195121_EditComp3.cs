using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class EditComp3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AutomaticReply",
                table: "Companies",
                newName: "AutomaticResponse");

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "Companies",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "AutomaticResponse",
                table: "Companies",
                newName: "AutomaticReply");
        }
    }
}
