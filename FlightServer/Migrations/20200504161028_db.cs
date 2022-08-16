using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightServer.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusStation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<bool>(nullable: false),
                    FlightId = table.Column<int>(nullable: true),
                    OptionalLandingStation = table.Column<string>(nullable: true),
                    OptionalFlightStation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentStationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_StatusStation_CurrentStationId",
                        column: x => x.CurrentStationId,
                        principalTable: "StatusStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(nullable: false),
                    StationId = table.Column<int>(nullable: false),
                    EntringTime = table.Column<DateTime>(nullable: true),
                    ExitTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightHistory_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightHistory_StatusStation_StationId",
                        column: x => x.StationId,
                        principalTable: "StatusStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlannedFlights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(nullable: false),
                    SourceStationId = table.Column<int>(nullable: false),
                    DestinationStationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedFlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedFlights_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlannedFlights_StatusStation_SourceStationId",
                        column: x => x.SourceStationId,
                        principalTable: "StatusStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlannedLanding",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(nullable: false),
                    SourceStationId = table.Column<int>(nullable: false),
                    DestinationStationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedLanding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedLanding_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlannedLanding_StatusStation_SourceStationId",
                        column: x => x.SourceStationId,
                        principalTable: "StatusStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightHistory_FlightId",
                table: "FlightHistory",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHistory_StationId",
                table: "FlightHistory",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentStationId",
                table: "Flights",
                column: "CurrentStationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedFlights_FlightId",
                table: "PlannedFlights",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedFlights_SourceStationId",
                table: "PlannedFlights",
                column: "SourceStationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedLanding_FlightId",
                table: "PlannedLanding",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedLanding_SourceStationId",
                table: "PlannedLanding",
                column: "SourceStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightHistory");

            migrationBuilder.DropTable(
                name: "PlannedFlights");

            migrationBuilder.DropTable(
                name: "PlannedLanding");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "StatusStation");
        }
    }
}
