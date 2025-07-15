using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class nodes_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4dcb74a2-39f1-4021-a3f8-a44153e14d37"), "Mail.API" },
                    { new Guid("5a3a9a4b-8baf-4875-939a-ba3eafef63fd"), "Stock.API" },
                    { new Guid("62648b9e-afa0-40f1-88dc-4c8787c1cbea"), "Payment.API" },
                    { new Guid("ee9ab40b-cc1f-419c-a758-1f993157008f"), "OrderAPI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("4dcb74a2-39f1-4021-a3f8-a44153e14d37"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("5a3a9a4b-8baf-4875-939a-ba3eafef63fd"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("62648b9e-afa0-40f1-88dc-4c8787c1cbea"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("ee9ab40b-cc1f-419c-a758-1f993157008f"));
        }
    }
}
