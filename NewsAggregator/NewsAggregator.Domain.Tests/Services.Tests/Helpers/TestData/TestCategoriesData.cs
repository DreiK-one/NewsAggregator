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
                        Id = Guid.NewGuid(),
                        Name= "Sport",
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name= "Culture",
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name= "Games",
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name= "Politic",
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name= "Social",
                    }
                }.AsQueryable();
            }
        }
    }
}
