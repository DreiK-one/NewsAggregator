using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IArticleServiceCQS
    {
        Task<ArticleDto> GetArticleById(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticles();
        Task<IEnumerable<ArticleDto>> GetArticlesByPage(int page);
        Task<IEnumerable<ArticleDto>> GetPositiveArticlesByPage(int page);
    }
}
