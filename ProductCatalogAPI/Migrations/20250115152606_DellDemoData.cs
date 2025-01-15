using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class DellDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Code",
                keyValue: "0001-0001");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Code",
                keyValue: "0002-0002");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Code",
                keyValue: "0003-0003");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Code", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { "0001-0001", "Description1", "prod1", 10.99m },
                    { "0002-0002", "Description2", "prod2", 20.50m },
                    { "0003-0003", "Description3", "prod3", 15.75m }
                });
        }
    }
}
