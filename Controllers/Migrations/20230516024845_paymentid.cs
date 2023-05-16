using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jenjennewborncare.Migrations
{
    /// <inheritdoc />
    public partial class paymentid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Invoices",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Invoices");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Invoices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
