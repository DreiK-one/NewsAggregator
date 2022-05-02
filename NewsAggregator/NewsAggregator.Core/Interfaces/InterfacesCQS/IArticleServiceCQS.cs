using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IArticleServiceCQS
    {
        Task<ArticleDto> GetArticleByIdForUser(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticlesForUser();
        Task<IEnumerable<ArticleDto>> GetArticlesByPageForUser(int page);

        Task<ArticleDto> GetArticleByIdForAdmin(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticlesForAdmin();
        Task<IEnumerable<ArticleDto>> GetArticlesByPageForAdmin(int page);
    }
}
