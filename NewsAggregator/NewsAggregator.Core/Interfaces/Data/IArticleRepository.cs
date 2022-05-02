using NewsAggregator.Data.Entities;


namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        public IEnumerable<Article> Get5TopRatedNewsOrderedByCreationDate();
    }
}
