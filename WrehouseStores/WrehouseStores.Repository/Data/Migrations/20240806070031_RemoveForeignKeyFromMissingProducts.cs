using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyFromMissingProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingProducts_Products_ProductId",
                table: "MissingProducts");

            migrationBuilder.DropIndex(
                name: "IX_MissingProducts_ProductId",
                table: "MissingProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
