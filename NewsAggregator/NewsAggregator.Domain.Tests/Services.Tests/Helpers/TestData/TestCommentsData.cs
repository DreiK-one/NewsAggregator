using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestCommentsData
    {
        public static IQueryable<Comment> Comments
        {
            get
            {
                return new List<Comment>
                {
                    new Comment
                    {
                        Id = new Guid("1d047401-95fb-4e18-a946-72c6a3062a7a")
                    },
                    new Comment
                    {
                        Id = new Guid("fbfa3257-71a5-445a-b58a-e9483aaec298")
                    },
                    new Comment
                    {
                        Id = new Guid("cff0f0c4-9116-487c-89de-a9afed6c08a1")
                    },
                    new Comment
                    {
                        Id = new Guid("ec3a18ed-33d9-4b93-a1f7-18c2bd7d1e8b")
                    },
                    new Comment
                    {
                        Id = new Guid("10a0cb86-4867-4d9a-8c40-7c6f36a28061")
                    }
                }.AsQueryable();
            }
        }
    }
}
