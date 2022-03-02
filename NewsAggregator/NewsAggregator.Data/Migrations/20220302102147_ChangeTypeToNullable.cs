using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class ChangeTypeToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Coefficient",
                table: "Articles",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Coefficient",
                table: "Articles",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
