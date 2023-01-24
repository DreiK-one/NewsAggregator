using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Helpers
{
    public static class TestArticlesData
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
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 1.6f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = new Guid("e076479a-a96f-40fe-a54f-ddaceec29558"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.7f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = new Guid("16c80b2d-db21-4b0f-aeb1-c2bc5e925e9a"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 2.3f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = new Guid("1ca88641-9f96-417c-ac2f-1c5f343a4080"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.8f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = new Guid("97f7d7fd-3d77-498d-a024-16c0354b6ccb"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2.9f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3.5f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.1f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 5f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3f,
                        SourceUrl = "onliner"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.1f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 1.5f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 0f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.3f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.5f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.7f,
                        SourceUrl = "goha"      
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.5f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 5.0f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3.5f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.5f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.6f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.3f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.9f,
                        SourceUrl = "goha"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.5f,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.2f,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.4f,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.6f,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = null,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = null,
                        SourceUrl = "lenta"
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2.1f,
                        SourceUrl = "lenta"
                    }

                }.AsQueryable();
            }
        }
    }
}
