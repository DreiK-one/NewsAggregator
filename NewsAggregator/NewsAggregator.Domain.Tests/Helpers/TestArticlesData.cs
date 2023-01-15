using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 2.3f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4.8f,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
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
