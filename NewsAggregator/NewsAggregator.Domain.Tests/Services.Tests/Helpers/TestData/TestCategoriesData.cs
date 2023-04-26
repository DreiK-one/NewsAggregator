using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestCategoriesData
    {
        public static IQueryable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category
                    {
                        Id = new Guid("16da24dc-6d12-45d1-b2c3-3b62d39cb3ea"),
                        Name= "Auto",
                    },
                    new Category
                    {
                        Id = new Guid("4c407f85-21a2-4503-a2d4-ba2a0c1c8582"),
                        Name= "People",
                    },
                    new Category
                    {
                        Id = new Guid("6f092abc-069a-4cc7-8fc4-bcae3a2a563d"),
                        Name= "Games",
                    },
                    new Category
                    {
                        Id = new Guid("762d4b33-3a29-4d0f-a46f-f4ac46d5b5d1"),
                        Name= "Politic",
                    },
                    new Category
                    {
                        Id = new Guid("452dd21e-139d-497a-9b41-dc9036f26fea"),
                        Name= "Social",
                    }
                }.AsQueryable();
            }
        }
    }
}
