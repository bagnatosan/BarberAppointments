using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Migrations
{
    /// <inheritdoc />
    public partial class RecurrentSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecurrentSchedulesId",
                table: "Appointments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurrentSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    HairdresserId = table.Column<int>(type: "INTEGER", nullable: false),
                    IntervalWeeks = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurrentSchedules_Hairdressers_HairdresserId",
                        column: x => x.HairdresserId,
                        principalTable: "Hairdressers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurrentSchedules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RecurrentSchedulesId",
                table: "Appointments",
                column: "RecurrentSchedulesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurrentSchedules_HairdresserId",
                table: "RecurrentSchedules",
                column: "HairdresserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurrentSchedules_UserId",
                table: "RecurrentSchedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_RecurrentSchedules_RecurrentSchedulesId",
                table: "Appointments",
                column: "RecurrentSchedulesId",
                principalTable: "RecurrentSchedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_RecurrentSchedules_RecurrentSchedulesId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "RecurrentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RecurrentSchedulesId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RecurrentSchedulesId",
                table: "Appointments");
        }
    }
}
