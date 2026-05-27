using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Migrations
{
    /// <inheritdoc />
    public partial class CambiarUserHairdresser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Hairdressers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hairdressers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Hairdressers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hairdressers_UserId",
                table: "Hairdressers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hairdressers_Users_UserId",
                table: "Hairdressers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hairdressers_Users_UserId",
                table: "Hairdressers");

            migrationBuilder.DropIndex(
                name: "IX_Hairdressers_UserId",
                table: "Hairdressers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hairdressers");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Hairdressers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hairdressers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
