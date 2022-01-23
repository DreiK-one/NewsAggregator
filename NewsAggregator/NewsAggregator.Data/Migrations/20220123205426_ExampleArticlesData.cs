using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    public partial class ExampleArticlesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "CategoryId", "Coefficient", "CreationDate", "Description", "SourceId", "Title" },
                values: new object[] { new Guid("1320bf13-6c2a-4394-aad7-d110ed1c95fc"), "Камерунский и французский боец смешанного стиля (MMA) Фрэнсис Нганну пожаловался на условия контракта с Абсолютным бойцовским чемпионатом (UFC). Об этом сообщает MMA Junkie.", new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"), 5f, new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8903), "Нганну пожаловался на условия контракта с UFC после защиты титула", new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81"), "Чемпион UFC Фрэнсис Нганну о контракте: я не чувствую себя свободным!" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "CategoryId", "Coefficient", "CreationDate", "Description", "SourceId", "Title" },
                values: new object[] { new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"), "В последние годы UFC, под эгидой которой проводятся бои по смешанным единоборствам, стабильно держится в топе зрительского интереса. Организация постоянно создает шоу и генерирует эмоции, которые привлекают даже тех, кто не интересуется спортом. Один конкретный бой может раскручиваться как эпохальное событие вселенского масштаба, которое нельзя проигнорировать. При этом непосвященные, включив трансляцию, могут остаться в недоумении: ну, молотят они друг друга в клетке — и что дальше-то? В рамках совместного спортивного спецпроекта Onlíner и  разбираемся, что собой представляет мир UFC и как в него погрузиться.", new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"), 3.5f, new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8838), "Краткий ликбез по смешанным единоборствам", new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"), "Как начать смотреть UFC и за кем следить, кроме Макгрегора?" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "CategoryId", "Coefficient", "CreationDate", "Description", "SourceId", "Title" },
                values: new object[] { new Guid("248a2c01-569e-4164-bd77-af80e6d89cb8"), "Взять и все поменять — каждый из нас задумывался о таком радикальном шаге хотя бы один раз в жизни. Скорее всего — намного больше. Потому что живем мы как-то неидеально, постоянно совершаем ошибки, не успеем их исправить, как уже новых наделаем — и, закопавшись под всем этим, в какой-то момент приходим в себя уже немолодыми и совсем не счастливыми. ", new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"), 4f, new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8897), "Абсолютно счастливый! В 42 года отказался от кресла руководителя крупной строительной компании и стал поваром", new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"), "Ушел с хорошей должности и стал поваром!" });
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

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "CategoryId", "Coefficient", "CreationDate", "Description", "SourceId", "Title" },
                values: new object[,]
                {
                    { new Guid("1b26e74c-f506-4471-b985-db518721cbfc"), "В последние годы UFC, под эгидой которой проводятся бои по смешанным единоборствам, стабильно держится в топе зрительского интереса. Организация постоянно создает шоу и генерирует эмоции, которые привлекают даже тех, кто не интересуется спортом. Один конкретный бой может раскручиваться как эпохальное событие вселенского масштаба, которое нельзя проигнорировать. При этом непосвященные, включив трансляцию, могут остаться в недоумении: ну, молотят они друг друга в клетке — и что дальше-то? В рамках совместного спортивного спецпроекта Onlíner и  разбираемся, что собой представляет мир UFC и как в него погрузиться.", new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"), 3.5f, new DateTime(2022, 1, 23, 21, 53, 7, 633, DateTimeKind.Local).AddTicks(4876), "Краткий ликбез по смешанным единоборствам", new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"), "Как начать смотреть UFC и за кем следить, кроме Макгрегора?" },
                    { new Guid("1ebbf6dc-d076-4429-b1cb-cca39cda5efb"), "Взять и все поменять — каждый из нас задумывался о таком радикальном шаге хотя бы один раз в жизни. Скорее всего — намного больше. Потому что живем мы как-то неидеально, постоянно совершаем ошибки, не успеем их исправить, как уже новых наделаем — и, закопавшись под всем этим, в какой-то момент приходим в себя уже немолодыми и совсем не счастливыми. ", new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"), 4f, new DateTime(2022, 1, 23, 21, 53, 7, 633, DateTimeKind.Local).AddTicks(4931), "Абсолютно счастливый! В 42 года отказался от кресла руководителя крупной строительной компании и стал поваром", new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"), "Ушел с хорошей должности и стал поваром!" },
                    { new Guid("c56eb416-1bac-4227-a455-0a3ecb52c49e"), "Камерунский и французский боец смешанного стиля (MMA) Фрэнсис Нганну пожаловался на условия контракта с Абсолютным бойцовским чемпионатом (UFC). Об этом сообщает MMA Junkie.", new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"), 5f, new DateTime(2022, 1, 23, 21, 53, 7, 633, DateTimeKind.Local).AddTicks(4938), "Нганну пожаловался на условия контракта с UFC после защиты титула", new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81"), "Чемпион UFC Фрэнсис Нганну о контракте: я не чувствую себя свободным!" }
                });
        }
    }
}
