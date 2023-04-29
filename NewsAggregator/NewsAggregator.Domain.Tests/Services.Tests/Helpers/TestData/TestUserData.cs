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
                        Id = new Guid("f1dc4182-9459-49ea-a4d2-98f928c6da98"),
                        Email = "andrew1993@gmail.com",
                        NormalizedEmail = "ANDREW1993@GMAIL.COM",
                        Nickname = "Andrew",
                        NormalizedNickname = "ANDREW"
                    },
                    new User
                    {
                        Id = new Guid("08fe00bc-6f69-48bf-9d52-19a74a7be2f6"),
                        Email = "mick1990@mail.ru",
                        NormalizedEmail = "MICK1990@MAIL.RU",
                        Nickname = "Mick",
                        NormalizedNickname = "MICK"
                    },
                    new User
                    {
                        Id = new Guid("51d741e4-3f36-481f-b895-e1ff4fcf4743"),
                        Email = "margo17@gmail.com",
                        NormalizedEmail = "MARGO17@GMAIL.COM",
                        Nickname = "Margo",
                        NormalizedNickname = "MARGO"
                    }
                }.AsQueryable();
            }
        }
    }
}
