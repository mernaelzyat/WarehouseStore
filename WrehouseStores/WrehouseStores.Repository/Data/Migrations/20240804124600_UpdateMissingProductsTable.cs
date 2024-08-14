using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMissingProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductsId",
                table: "MissingProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MissingProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductsId",
                table: "MissingProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MissingProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
