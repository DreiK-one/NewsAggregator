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
                    },
                    new Article
                    {
                        Id = new Guid("e076479a-a96f-40fe-a54f-ddaceec29558"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.7f,
                    },
                    new Article
                    {
                        Id = new Guid("16c80b2d-db21-4b0f-aeb1-c2bc5e925e9a"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 2.3f,
                    },
                    new Article
                    {
                        Id = new Guid("1ca88641-9f96-417c-ac2f-1c5f343a4080"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.8f,
                    },
                    new Article
                    {
                        Id = new Guid("97f7d7fd-3d77-498d-a024-16c0354b6ccb"),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2.9f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.4f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.1f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 1.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 0f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.3f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 5.0f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -3.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.6f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.3f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -5.0f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.5f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 3.2f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.7f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -4.6f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2.3f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1.1f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2.1f,
                    }

                }.AsQueryable();
            }
        }
    }
}
