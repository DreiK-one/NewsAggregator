using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddSourceUrlToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Articles");
        }
    }
}
