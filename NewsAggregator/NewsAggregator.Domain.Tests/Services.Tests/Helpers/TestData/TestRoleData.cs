using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData
{
    public static class TestRoleData
    {
        public static IQueryable<Role> Roles
        {
            get
            {
                return new List<Role>
                {
                    new Role
                    {
                        Id = new Guid("f1dc4182-9459-49ea-a4d2-98f928c6da98"),
                        Name = "Admin"
                    },
                    new Role
                    {
                        Id = new Guid("08fe00bc-6f69-48bf-9d52-19a74a7be2f6"),
                        Name = "User"
                    },
                    new Role
                    {
                        Id = new Guid("51d741e4-3f36-481f-b895-e1ff4fcf4743"),
                        Name = "Manager"
                    }
                }.AsQueryable();
            }
        }
    }
}
