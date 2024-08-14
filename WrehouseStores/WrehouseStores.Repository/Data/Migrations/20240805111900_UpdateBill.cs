using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseStores.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Bill",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_EmployeeId",
                table: "Bill",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Employees_EmployeeId",
                table: "Bill",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Employees_EmployeeId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_EmployeeId",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Bill");
        }
    }
}
