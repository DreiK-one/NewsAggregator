using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestHtmlParserData
    {
        public static IQueryable<Article> Articles
        {
            get
            {
                return new List<Article>
                {
                    new Article
                    {
                        Id = new Guid("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c"),
                        Body = "",
                        Title = "",
                        Description = "",
                        CreationDate = DateTime.Now,
                        Coefficient = 1.6f,
                        SourceUrl = "https://auto.onliner.by/2023/02/24/v-minske-mogut-zapretit-parkovku-na-ulicax-po-opredelennym-dnyam-zimoj"
                    },
                    new Article
                    {
                        Id = new Guid("e076479a-a96f-40fe-a54f-ddaceec29558"),
                        Body = "",
                        Title = "",
                        Description = "",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.7f,
                        SourceUrl = "https://www.goha.ru/v-ekranizacii-fallout-snimetsya-ella-pernell-dzhinks-iz-arkejn-lO1vLE"
                    },
                    new Article
                    {
                        Id = new Guid("16c80b2d-db21-4b0f-aeb1-c2bc5e925e9a"),
                        Body = "",
                        Title = "",
                        Description = "",
                        CreationDate = DateTime.Now,
                        Coefficient = 2.3f,
                        SourceUrl = "https://shazoo.ru/2022/04/04/124743/razrabotcik-abandoned-rasskazal-o-kontaktax-s-konami"
                    }
                }.AsQueryable();
            }
        }
    }
}
