using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class logs1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelLog_Vehicles_VehicleId",
                table: "FuelLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelLog",
                table: "FuelLog");

            migrationBuilder.RenameTable(
                name: "FuelLog",
                newName: "FuelLogs");

            migrationBuilder.RenameIndex(
                name: "IX_FuelLog_VehicleId",
                table: "FuelLogs",
                newName: "IX_FuelLogs_VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelLogs",
                table: "FuelLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelLogs_Vehicles_VehicleId",
                table: "FuelLogs",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelLogs_Vehicles_VehicleId",
                table: "FuelLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelLogs",
                table: "FuelLogs");

            migrationBuilder.RenameTable(
                name: "FuelLogs",
                newName: "FuelLog");

            migrationBuilder.RenameIndex(
                name: "IX_FuelLogs_VehicleId",
                table: "FuelLog",
                newName: "IX_FuelLog_VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelLog",
                table: "FuelLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelLog_Vehicles_VehicleId",
                table: "FuelLog",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
