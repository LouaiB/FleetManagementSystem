using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetApi1.Migrations
{
    public partial class deliverySummary1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Drivers",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phonenumber",
                table: "Drivers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Drivers",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Birthdate",
                table: "Drivers",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Drivers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Drivers",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Companies",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AutomaticResponse",
                table: "Companies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ManagerId",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "Companies",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Companies",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Clients",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phonenumber",
                table: "Clients",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clients",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Birthdate",
                table: "Clients",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Clients",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Period = table.Column<int>(nullable: false),
                    PlanId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicensePlate = table.Column<string>(maxLength: 50, nullable: false),
                    Make = table.Column<string>(maxLength: 50, nullable: false),
                    Model = table.Column<string>(maxLength: 50, nullable: false),
                    Longtitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    CompanyId = table.Column<long>(nullable: true),
                    CurrentDriverId = table.Column<long>(nullable: true),
                    isCurrentlyActive = table.Column<bool>(nullable: false),
                    purchaseDate = table.Column<DateTime>(type: "Date", nullable: false),
                    PayLoad = table.Column<int>(nullable: false),
                    EmissionsCO2 = table.Column<int>(nullable: false),
                    FuelConsumption = table.Column<int>(nullable: false),
                    FuelLevel = table.Column<int>(nullable: false),
                    CurrentLoad = table.Column<int>(nullable: false),
                    Odometer = table.Column<float>(nullable: false),
                    PlanId = table.Column<long>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ManagerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Drivers_CurrentDriverId",
                        column: x => x.CurrentDriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MapLocations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Langtitude = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    RouteId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapLocations_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Service = table.Column<string>(maxLength: 100, nullable: false),
                    DateTime = table.Column<DateTime>(type: "Date", nullable: false),
                    Cost = table.Column<float>(nullable: false),
                    VehicleId = table.Column<long>(nullable: true),
                    Provider = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<long>(nullable: true),
                    ClientId = table.Column<long>(nullable: true),
                    VehicleId = table.Column<long>(nullable: true),
                    DriverId = table.Column<long>(nullable: true),
                    SourceLongtitude = table.Column<double>(nullable: false),
                    SourceLatitude = table.Column<double>(nullable: false),
                    SourceCity = table.Column<string>(nullable: true),
                    DestinationLongtitude = table.Column<double>(nullable: false),
                    DestinationLatitude = table.Column<double>(nullable: false),
                    DestinationCity = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Answered = table.Column<bool>(nullable: false),
                    Started = table.Column<bool>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    OptimalDistance = table.Column<float>(nullable: false),
                    OptimalTime = table.Column<int>(nullable: false),
                    OptimalFuelConsumption = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledActivities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicleId = table.Column<long>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledActivities_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliverySummaries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeliveryId = table.Column<long>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartFuelLevel = table.Column<float>(nullable: false),
                    EndFuelLevel = table.Column<float>(nullable: false),
                    StartOdometer = table.Column<float>(nullable: false),
                    EndOdometer = table.Column<float>(nullable: false),
                    HarshAccelerationAndDeceleration = table.Column<float>(nullable: false),
                    HarshBreakingsRate = table.Column<float>(nullable: false),
                    HardCorneringRate = table.Column<float>(nullable: false),
                    SpeedingsRate = table.Column<float>(nullable: false),
                    SeatBeltRate = table.Column<float>(nullable: false),
                    OverRevving = table.Column<float>(nullable: false),
                    OnTimeDeliveryRate = table.Column<float>(nullable: false),
                    FuelConsumptionRate = table.Column<float>(nullable: false),
                    Idling = table.Column<float>(nullable: false),
                    PerformanceScore = table.Column<float>(nullable: false),
                    ComplianceScore = table.Column<float>(nullable: false),
                    SafetyScore = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverySummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliverySummaries_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ManagerId",
                table: "Companies",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PlanId",
                table: "Activities",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VehicleId",
                table: "Bills",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ClientId",
                table: "Deliveries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CompanyId",
                table: "Deliveries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DriverId",
                table: "Deliveries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_VehicleId",
                table: "Deliveries",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverySummaries_DeliveryId",
                table: "DeliverySummaries",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_MapLocations_RouteId",
                table: "MapLocations",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledActivities_ActivityId",
                table: "ScheduledActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledActivities_VehicleId",
                table: "ScheduledActivities",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CurrentDriverId",
                table: "Vehicles",
                column: "CurrentDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ManagerId",
                table: "Vehicles",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlanId",
                table: "Vehicles",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Managers_ManagerId",
                table: "Companies",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Managers_ManagerId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "DeliverySummaries");

            migrationBuilder.DropTable(
                name: "MapLocations");

            migrationBuilder.DropTable(
                name: "ScheduledActivities");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ManagerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AutomaticResponse",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Phonenumber",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Birthdate",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(long))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Phonenumber",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Birthdate",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
