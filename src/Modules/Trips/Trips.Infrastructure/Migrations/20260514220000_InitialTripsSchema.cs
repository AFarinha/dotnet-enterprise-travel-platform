using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trips.Infrastructure.Migrations;

public partial class InitialTripsSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "trips");

        migrationBuilder.CreateTable(
            name: "Trips",
            schema: "trips",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                OwnerUserId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                DestinationCountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                DestinationCity = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                StartsOn = table.Column<DateOnly>(type: "date", nullable: true),
                EndsOn = table.Column<DateOnly>(type: "date", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Trips", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Travellers",
            schema: "trips",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FirstName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                LastName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                Email = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                TripId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Travellers", x => x.Id);
                table.ForeignKey("FK_Travellers_Trips_TripId", x => x.TripId, principalSchema: "trips", principalTable: "Trips", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TripTimeline",
            schema: "trips",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                EventType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                TripId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TripTimeline", x => x.Id);
                table.ForeignKey("FK_TripTimeline_Trips_TripId", x => x.TripId, principalSchema: "trips", principalTable: "Trips", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(name: "IX_Travellers_TripId", schema: "trips", table: "Travellers", column: "TripId");
        migrationBuilder.CreateIndex(name: "IX_TripTimeline_TripId", schema: "trips", table: "TripTimeline", column: "TripId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Travellers", schema: "trips");
        migrationBuilder.DropTable(name: "TripTimeline", schema: "trips");
        migrationBuilder.DropTable(name: "Trips", schema: "trips");
    }
}
