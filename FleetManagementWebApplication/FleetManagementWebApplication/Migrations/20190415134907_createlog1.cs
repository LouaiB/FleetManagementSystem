using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class createlog1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(nullable: false),
                    VehicleId = table.Column<long>(nullable: false),
                    DriverId = table.Column<long>(nullable: false),
                    Langtitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Fuel = table.Column<double>(nullable: false),
                    Speed = table.Column<int>(nullable: false),
                    Odometer = table.Column<double>(nullable: false),
                    Seatbelt = table.Column<bool>(nullable: false),
                    Harshbreak = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleLogs_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLogs_DriverId",
                table: "VehicleLogs",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLogs_VehicleId",
                table: "VehicleLogs",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleLogs");
        }
    }
}
