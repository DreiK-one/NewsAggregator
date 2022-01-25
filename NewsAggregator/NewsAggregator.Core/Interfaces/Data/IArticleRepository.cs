using NewsAggregator.Data.Entities;
using System.Linq.Expressions;


namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        public IEnumerable<Article> Get5TopRatedNewsOrderedByCreationDate();
    }
}
