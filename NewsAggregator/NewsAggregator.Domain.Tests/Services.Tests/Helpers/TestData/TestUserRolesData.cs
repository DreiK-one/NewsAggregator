using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestUserRolesData
    {
        public static IQueryable<UserRole> UserRoles
        {
            get
            {
                return new List<UserRole>
                {
                    new UserRole
                    {
                        Id = new Guid("232fdbde-0edf-436a-9629-eaa89931156e"),
                        UserId = new Guid("f1dc4182-9459-49ea-a4d2-98f928c6da98"),
                        RoleId = new Guid("bca65099-4302-49d7-b1a3-4d23d031739d"),
                        Role = new Role
                        {
                            Name = "User"
                        }

                    },
                    new UserRole
                    {
                        Id = new Guid("67f3248c-d78a-426d-b8f8-8d98ae21769d"),
                        UserId = new Guid("08fe00bc-6f69-48bf-9d52-19a74a7be2f6"),
                        RoleId = new Guid("f5f2fff5-62f7-486c-a1d6-314e5d99a36c"),
                        Role = new Role
                        {
                            Name = "Admin"
                        }
                    }
                }.AsQueryable();
            }
        }
    }
}
