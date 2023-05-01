using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jenjennewborncare.Migrations
{
    /// <inheritdoc />
    public partial class nannie3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Nannies",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Nannies");
        }
    }
}
