using NewsAggregator.Data.Entities;


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
        IBaseRepository<UserRole> UserRoles { get; }
        IBaseRepository<RefreshToken> RefreshTokens { get; }

        Task<int> Save();
    }
}
