using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLogicAgency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OffDay",
                table: "AgencySettings");

            migrationBuilder.AddColumn<long>(
                name: "AgencyId",
                table: "Appointments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AgencyId",
                table: "AgencySettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgencyHolidays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyId = table.Column<long>(type: "bigint", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyHolidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyHolidays_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AgencyId",
                table: "Appointments",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencySettings_AgencyId",
                table: "AgencySettings",
                column: "AgencyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyHolidays_AgencyId",
                table: "AgencyHolidays",
                column: "AgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencySettings_Agencies_AgencyId",
                table: "AgencySettings",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Agencies_AgencyId",
                table: "Appointments",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencySettings_Agencies_AgencyId",
                table: "AgencySettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Agencies_AgencyId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AgencyHolidays");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AgencyId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_AgencySettings_AgencyId",
                table: "AgencySettings");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "AgencySettings");

            migrationBuilder.AddColumn<DateTime>(
                name: "OffDay",
                table: "AgencySettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
