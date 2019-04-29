using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class deliverySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfHarshbreaks",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "NumberOfNoSeatbelts",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "NumberOfSpeedings",
                table: "DeliverySummaries");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "ComplianceScore",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FuelConsumptionRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HardCorneringRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HarshAccelerationAndDeceleration",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HarshBreakingsRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Idling",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OnTimeDeliveryRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OverRevving",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PerformanceScore",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SafetyScore",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SeatBeltRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SpeedingsRate",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ComplianceScore",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "FuelConsumptionRate",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "HardCorneringRate",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "HarshAccelerationAndDeceleration",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "HarshBreakingsRate",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "Idling",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "OnTimeDeliveryRate",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "OverRevving",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "PerformanceScore",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "SafetyScore",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "SeatBeltRate",
                table: "DeliverySummaries");

            migrationBuilder.DropColumn(
                name: "SpeedingsRate",
                table: "DeliverySummaries");

            migrationBuilder.AddColumn<bool>(
                name: "NumberOfHarshbreaks",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NumberOfNoSeatbelts",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSpeedings",
                table: "DeliverySummaries",
                nullable: false,
                defaultValue: 0);
        }
    }
}
