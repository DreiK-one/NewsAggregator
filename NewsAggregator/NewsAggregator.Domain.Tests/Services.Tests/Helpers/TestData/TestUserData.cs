using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestUserData
    {
        public static IQueryable<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User
                    {

                    },
                    new User
                    {

                    },
                    new User
                    {

                    }
                }.AsQueryable();
            }
        }
    }
}
