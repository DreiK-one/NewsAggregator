using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddNewCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("338b97c5-6a3c-40d7-a4e4-1aa542685883"), "Other" },
                    { new Guid("41dd49d2-d3ee-4329-951e-430551fafca0"), "Realt" },
                    { new Guid("5de4aa36-8072-49ae-bf5e-80c3b381aeeb"), "Auto" },
                    { new Guid("80dc4c7b-2d21-4d0d-906c-81716327b4fa"), "Money" },
                    { new Guid("d857373d-7efc-4c7e-80db-0ba6507c87a8"), "Tech" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("338b97c5-6a3c-40d7-a4e4-1aa542685883"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("41dd49d2-d3ee-4329-951e-430551fafca0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5de4aa36-8072-49ae-bf5e-80c3b381aeeb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("80dc4c7b-2d21-4d0d-906c-81716327b4fa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d857373d-7efc-4c7e-80db-0ba6507c87a8"));
        }
    }
}
