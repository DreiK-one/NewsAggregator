using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IArticleServiceCQS
    {
        Task<ArticleDto> GetArticleById(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticles(int? page, string? role);
    }
}
