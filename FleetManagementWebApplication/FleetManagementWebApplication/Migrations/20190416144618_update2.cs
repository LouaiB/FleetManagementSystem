using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<float>(
                name: "Odometer",
                table: "VehicleLogs",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Fuel",
                table: "VehicleLogs",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "Vehicles");

            migrationBuilder.AlterColumn<double>(
                name: "Odometer",
                table: "VehicleLogs",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Fuel",
                table: "VehicleLogs",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
