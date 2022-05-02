using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(NewsAggregatorContext context) : base(context)
        {

        }

        public override async Task<Article> GetById(Guid id)
        {
            return await DbSet
                .Where(article => !string.IsNullOrEmpty(article.Body))
                .FirstOrDefaultAsync(article => article.Id.Equals(id));
        }

        public IEnumerable<Article> Get5TopRatedNewsOrderedByCreationDate()
        {
            return DbSet.OrderByDescending(article => article.Coefficient).Take(5)
                .OrderBy(article => article.CreationDate).ToList();
        }
    }
}
