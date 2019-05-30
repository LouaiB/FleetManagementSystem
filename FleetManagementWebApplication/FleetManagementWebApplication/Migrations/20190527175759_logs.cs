using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FleetManagementWebApplication.Migrations
{
    public partial class logs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    VehicleId = table.Column<long>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    PricePerLitre = table.Column<float>(nullable: false),
                    Provider = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelLog_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelLog_VehicleId",
                table: "FuelLog",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelLog");
        }
    }
}
