using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Migrations
{
    /// <inheritdoc />
    public partial class addmodelcreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_HairdresserId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HairdresserId_Date",
                table: "Appointments",
                columns: new[] { "HairdresserId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_HairdresserId_Date",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Appointments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HairdresserId",
                table: "Appointments",
                column: "HairdresserId");
        }
    }
}
