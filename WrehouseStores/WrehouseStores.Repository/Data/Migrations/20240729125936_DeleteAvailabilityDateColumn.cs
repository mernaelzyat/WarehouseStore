using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAvailabilityDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityDate",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "DateOfNeed",
                table: "PurchaseOrders",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PurchaseOrders",
                newName: "DateOfNeed");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AvailabilityDate",
                table: "PurchaseOrders",
                type: "date",
                nullable: true);
        }
    }
}
