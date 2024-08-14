using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMissingProductsTableLast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts");

            migrationBuilder.DropIndex(
                name: "IX_MissingProducts_ProductsId",
                table: "MissingProducts");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "MissingProducts");

            migrationBuilder.CreateIndex(
                name: "IX_MissingProducts_ProductId",
                table: "MissingProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissingProducts_Products_ProductId",
                table: "MissingProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingProducts_Products_ProductId",
                table: "MissingProducts");

            migrationBuilder.DropIndex(
                name: "IX_MissingProducts_ProductId",
                table: "MissingProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "MissingProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MissingProducts_ProductsId",
                table: "MissingProducts",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
