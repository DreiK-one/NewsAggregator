using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IArticleServiceCQS
    {
        Task<IEnumerable<ArticleDto>> GetAllArticles(int? page, string? role);
        Task<ArticleDto> GetArticleById(Guid id);

        Task<float?> MaxCoefOfToday();
        Task<float?> MaxCoefOfTheMonth();
        Task<float?> MaxCoefOfAllTime();
    }
}
