using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestSourcesData
    {
        public static IQueryable<Source> Sources
        {
            get
            {
                return new List<Source>()
                {
                    new Source()
                    {
                        Id = new Guid("cdb6ae1b-36f0-4dae-832c-1315e1e0634a"),
                        BaseUrl = "onliner.by",
                        RssUrl = "http://onliner.by/feed"
                    },
                    new Source
                    {
                        Id = new Guid("31ecd07b-8e20-42c0-8b12-8d3f0b7b2be2"),
                        BaseUrl = "goha.ru",
                        RssUrl = "http://goha.ru/feed"
                    },
                    new Source
                    {
                        Id = new Guid("119af140-9ccb-42e8-9d03-d0ae8f362b51"),
                        BaseUrl = "shazoo.ru",
                        RssUrl = "http://shazoo.ru/feed"
                    }
                }.AsQueryable();
            }
        }
    }
}
