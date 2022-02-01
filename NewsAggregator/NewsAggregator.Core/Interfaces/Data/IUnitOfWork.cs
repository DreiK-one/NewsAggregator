using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Comment> Comments { get; }
        IBaseRepository<Role> Roles { get; }
        IBaseRepository<Source> Sources { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<UserActivity> UserActivities { get; }

        Task<int> Save();
    }
}
