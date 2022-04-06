using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class AddExampleUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Nickname", "NormalizedEmail", "NormalizedNickname", "PasswordHash", "RegistrationDate" },
                values: new object[] { new Guid("c46d09f1-f535-4822-9372-dc3af86672fb"), "123@mail.ru", "Tom", "123@MAIL.RU", "TOM", "123", new DateTime(2022, 2, 17, 9, 57, 5, 34, DateTimeKind.Local).AddTicks(5860) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c46d09f1-f535-4822-9372-dc3af86672fb"));
        }
    }
}
