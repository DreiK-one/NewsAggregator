using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;

namespace NewsAggregator.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext _db;

        private readonly IArticleRepository _articleRepository;

        public UnitOfWork(IArticleRepository articleRepository, NewsAggregatorContext context)
        {
            _articleRepository = articleRepository;
            _db = context;
        }

        public IArticleRepository Articles => _articleRepository;

        public object Roles { get; }
        public object Users { get; }
        public object Sources { get; }
        public object Comments { get; }
        public object Categories { get; set; }


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