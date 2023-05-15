using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPITest.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DayStart",
                table: "Bookings",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "DayEnd",
                table: "Bookings",
                newName: "DateEnd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Bookings",
                newName: "DayStart");

            migrationBuilder.RenameColumn(
                name: "DateEnd",
                table: "Bookings",
                newName: "DayEnd");
        }
    }
}
