using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddImageAndSourceUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("1320bf13-6c2a-4394-aad7-d110ed1c95fc"),
                column: "Image",
                value: "background-image: url(https://fighttime.ru/media/k2/items/cache/8f24ade3a588e8d943f33b6f45e3cc72_XL.jpg);"
                );
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("1320bf13-6c2a-4394-aad7-d110ed1c95fc"),
                column: "SourceUrl",
                value: "https://lenta.ru/news/2022/01/23/ngannoe_contract/"
                );

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"),
                column: "Image",
                value: "background-image: url(https://content.onliner.by/news/1400x5616/76cac5474ed93c25780ff4ed353122a7.jpeg);"
                );
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"),
                column: "SourceUrl",
                value: "https://people.onliner.by/2022/01/20/kak-nachat-smotret-ufc"
                );

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("248a2c01-569e-4164-bd77-af80e6d89cb8"),
                column: "Image",
                value: "background-image: url(https://content.onliner.by/news/1400x5616/340051264c53b5a7e453b0185283c405.jpeg);"
                );
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("248a2c01-569e-4164-bd77-af80e6d89cb8"),
                column: "SourceUrl",
                value: "https://people.onliner.by/opinions/2022/01/20/stal-povarom"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("1320bf13-6c2a-4394-aad7-d110ed1c95fc"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("248a2c01-569e-4164-bd77-af80e6d89cb8"));

        }
    }
}
