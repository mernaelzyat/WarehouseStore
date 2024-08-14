using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingProductsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "MissingProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MissingProducts_ProductsId",
                table: "MissingProducts",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissingProducts_Products_ProductsId",
                table: "MissingProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
