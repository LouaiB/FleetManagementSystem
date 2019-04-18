using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class createDelivery2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OptimalDistance",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OptimalFuelConsumption",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "OptimalTime",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptimalDistance",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "OptimalFuelConsumption",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "OptimalTime",
                table: "Deliveries");
        }
    }
}
