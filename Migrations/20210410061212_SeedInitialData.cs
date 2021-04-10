using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleCrudAPI.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "ID", "Name", "Phone" },
                values: new object[] { 1, "Fake Supplier #1", "123456789" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Email", "IsAdmin" },
                values: new object[] { 1, "admin@test.com", true });

            migrationBuilder.InsertData(
                table: "Receipts",
                columns: new[] { "ID", "Amount", "Comments", "Date", "SupplierID", "UserID" },
                values: new object[] { 1, 1000m, "First Receipt!", new DateTime(2021, 4, 10, 1, 12, 12, 271, DateTimeKind.Local).AddTicks(9878), 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Receipts",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
