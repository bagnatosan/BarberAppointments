using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Migrations
{
    /// <inheritdoc />
    public partial class nullableobjetosappointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Appointments",
                type: "TEXT",
                nullable: true);
        }
    }
}
