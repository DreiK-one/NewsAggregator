using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddExampleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"), "People" },
                    { new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"), "Sport" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0cd86cd8-36b8-408e-9632-f336028c9e2f"), "User" },
                    { new Guid("50409b8d-9c49-40a6-9d94-97e94db38819"), "Moderator" },
                    { new Guid("fcbb86d1-0ca5-4aec-94db-e932b89a30b0"), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "BaseUrl", "Name", "RssUrl" },
                values: new object[,]
                {
                    { new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81"), "4pda.to", "4pda", "https://4pda.to/feed/" },
                    { new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"), "onliner.by", "Onliner", "https://www.onliner.by/feed" },
                    { new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b4"), "ria.ru", "Ria news", "https://ria.ru/export/rss2/archive/index.xml" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0cd86cd8-36b8-408e-9632-f336028c9e2f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("50409b8d-9c49-40a6-9d94-97e94db38819"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("fcbb86d1-0ca5-4aec-94db-e932b89a30b0"));

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81"));

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"));
        }
    }
}
