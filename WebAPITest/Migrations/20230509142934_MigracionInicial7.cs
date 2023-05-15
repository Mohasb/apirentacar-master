using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPITest.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Card",
                table: "Bookings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Card",
                table: "Bookings");
        }
    }
}
