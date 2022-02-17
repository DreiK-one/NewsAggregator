using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddExampleComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "CreationDate", "Text", "UserId" },
                values: new object[] { new Guid("97e4fcc3-f30c-40d2-bd1b-516a04db5cbf"), new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"), new DateTime(2022, 2, 17, 10, 5, 44, 662, DateTimeKind.Local).AddTicks(9018), "Интересная статья!", new Guid("c46d09f1-f535-4822-9372-dc3af86672fb") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("97e4fcc3-f30c-40d2-bd1b-516a04db5cbf"));
        }
    }
}
