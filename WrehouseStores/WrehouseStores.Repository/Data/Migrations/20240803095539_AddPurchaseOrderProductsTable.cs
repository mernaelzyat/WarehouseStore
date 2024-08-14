using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsPurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ReadyOrderProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "PurchaseOrderProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderProducts_ProductId",
                table: "PurchaseOrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId",
                table: "PurchaseOrderProducts",
                column: "PurchaseOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ReadyOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductsPurchaseOrders",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsPurchaseOrders", x => new { x.ProductsId, x.PurchaseOrdersId });
                    table.ForeignKey(
                        name: "FK_ProductsPurchaseOrders_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsPurchaseOrders_PurchaseOrders_PurchaseOrdersId",
                        column: x => x.PurchaseOrdersId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsPurchaseOrders_PurchaseOrdersId",
                table: "ProductsPurchaseOrders",
                column: "PurchaseOrdersId");
        }
    }
}
