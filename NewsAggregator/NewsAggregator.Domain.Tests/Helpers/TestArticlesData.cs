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
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 1,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -1,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 2,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = 4,
                    },
                    new Article
                    {
                        Id = Guid.NewGuid(),
                        Body = "examp",
                        CreationDate = DateTime.Now,
                        Coefficient = -2,
                    }
                }.AsQueryable();
            }
        }
    }
}
