using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data.Configuration
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(
                new Article
                {
                    Id = Guid.NewGuid(),
                    Title = "Как начать смотреть UFC и за кем следить, кроме Макгрегора?",
                    Description = "Краткий ликбез по смешанным единоборствам",
                    Body = "В последние годы UFC, под эгидой которой проводятся бои по смешанным единоборствам, стабильно держится в топе зрительского интереса. Организация постоянно создает шоу и генерирует эмоции, которые привлекают даже тех, кто не интересуется спортом. Один конкретный бой может раскручиваться как эпохальное событие вселенского масштаба, которое нельзя проигнорировать. При этом непосвященные, включив трансляцию, могут остаться в недоумении: ну, молотят они друг друга в клетке — и что дальше-то? В рамках совместного спортивного спецпроекта Onlíner и  разбираемся, что собой представляет мир UFC и как в него погрузиться.",
                    CreationDate = DateTime.Now,
                    Coefficient = 3.5f,
                    CategoryId = new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"),
                    SourceId = new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5") 
                },
                new Article
                {
                    Id = Guid.NewGuid(),
                    Title = "Ушел с хорошей должности и стал поваром!",
                    Description = "Абсолютно счастливый! В 42 года отказался от кресла руководителя крупной строительной компании и стал поваром",
                    Body = "Взять и все поменять — каждый из нас задумывался о таком радикальном шаге хотя бы один раз в жизни. Скорее всего — намного больше. Потому что живем мы как-то неидеально, постоянно совершаем ошибки, не успеем их исправить, как уже новых наделаем — и, закопавшись под всем этим, в какой-то момент приходим в себя уже немолодыми и совсем не счастливыми. ",
                    CreationDate = DateTime.Now,
                    Coefficient = 4.0f,
                    CategoryId = new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"),
                    SourceId = new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5")
                },
                new Article
                {
                    Id = Guid.NewGuid(),
                    Title = "Чемпион UFC Фрэнсис Нганну о контракте: я не чувствую себя свободным!",
                    Description = "Нганну пожаловался на условия контракта с UFC после защиты титула",
                    Body = "Камерунский и французский боец смешанного стиля (MMA) Фрэнсис Нганну пожаловался на условия контракта с Абсолютным бойцовским чемпионатом (UFC). Об этом сообщает MMA Junkie.",
                    CreationDate = DateTime.Now,
                    Coefficient = 5.0f,
                    CategoryId = new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"),
                    SourceId = new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81")

                });
        }
    }
}