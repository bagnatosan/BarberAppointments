using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Migrations
{
    /// <inheritdoc />
    public partial class PriceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HaircutId",
                table: "Appointments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Haircut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Haircut", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HaircutId",
                table: "Appointments",
                column: "HaircutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Haircut_HaircutId",
                table: "Appointments",
                column: "HaircutId",
                principalTable: "Haircut",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Haircut_HaircutId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Haircut");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_HaircutId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "HaircutId",
                table: "Appointments");
        }
    }
}
