using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class Maintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Odometer",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(decimal));

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
                name: "ScheduledActivities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicleId = table.Column<long>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false)
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
                name: "ServiceLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicleId = table.Column<long>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Provider = table.Column<string>(maxLength: 100, nullable: false),
                    Cost = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLogs_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PlanId",
                table: "Activities",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledActivities_ActivityId",
                table: "ScheduledActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledActivities_VehicleId",
                table: "ScheduledActivities",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_ActivityId",
                table: "ServiceLogs",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_VehicleId",
                table: "ServiceLogs",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledActivities");

            migrationBuilder.DropTable(
                name: "ServiceLogs");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.AlterColumn<decimal>(
                name: "Odometer",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
