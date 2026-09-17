using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrunStokPaneli.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialStockQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialStockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialStockQuantity",
                table: "Products");
        }
    }
}
