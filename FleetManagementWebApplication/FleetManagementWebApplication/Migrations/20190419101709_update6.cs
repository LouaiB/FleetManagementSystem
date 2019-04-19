using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Clients_ClientId",
                table: "Deliveries");

            migrationBuilder.AlterColumn<long>(
                name: "ClientId",
                table: "Deliveries",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Clients_ClientId",
                table: "Deliveries",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Clients_ClientId",
                table: "Deliveries");

            migrationBuilder.AlterColumn<long>(
                name: "ClientId",
                table: "Deliveries",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Clients_ClientId",
                table: "Deliveries",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
