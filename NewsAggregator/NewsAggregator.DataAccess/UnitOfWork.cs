using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregator.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext _db;

        private readonly IArticleRepository _articleRepository;

        private readonly IBaseRepository<Category> _categoryRepository;

        private readonly IBaseRepository<Comment> _commentRepository;

        private readonly IBaseRepository<Role> _roleRepository;

        private readonly IBaseRepository<Source> _sourceRepository;

        private readonly IBaseRepository<User> _userRepository;

        private readonly IBaseRepository<UserActivity> _userActivityRepository;

        private readonly IBaseRepository<UserRole> _userRoleRepository;

        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;


        public UnitOfWork(NewsAggregatorContext context, IArticleRepository articleRepository,
            IBaseRepository<Category> categories, IBaseRepository<Comment> comments,
            IBaseRepository<Role> roles, IBaseRepository<Source> sources,
            IBaseRepository<User> users, IBaseRepository<UserActivity> userActivities,
            IBaseRepository<UserRole> userRoleRepository, 
            IBaseRepository<RefreshToken> refreshTokenRepository)
        {
            _db = context;
            _articleRepository = articleRepository;
            _categoryRepository = categories;
            _commentRepository = comments;
            _roleRepository = roles;
            _sourceRepository = sources;
            _userRepository = users;
            _userActivityRepository = userActivities;
            _userRoleRepository = userRoleRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public IArticleRepository Articles => _articleRepository;
        public IBaseRepository<Category> Categories => _categoryRepository;
        public IBaseRepository<Comment> Comments => _commentRepository;
        public IBaseRepository<Role> Roles => _roleRepository;
        public IBaseRepository<Source> Sources => _sourceRepository;
        public IBaseRepository<User> Users => _userRepository;
        public IBaseRepository<UserActivity> UserActivities => _userActivityRepository;
        public IBaseRepository<UserRole> UserRoles => _userRoleRepository;
        public IBaseRepository<RefreshToken> RefreshTokens => _refreshTokenRepository;



        public async Task<int> Save()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            _articleRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}