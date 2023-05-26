using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IArticleServiceCQS
    {
        Task<IEnumerable<ArticleDto>> GetAllArticles(int? page, string? role);
        Task<ArticleDto> GetArticleById(Guid id);
        Task<int?> CreateAsync(CreateOrEditArticleDto articleDto);
        Task<int?> UpdateAsync(CreateOrEditArticleDto articleDto);
        Task<int?> DeleteAsync(Guid modelId);
        Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id);
        Task<ArticleDto> GetArticleWithAllNavigationPropertiesByRating(Guid id);
        Task<List<string>> GetAllExistingArticleUrls();
        Task<ArticleDto> GetArticleWithoutRating();
        Task<IEnumerable<ArticleDto>> GetAllNewsByRatingAsync();
        Task<IEnumerable<ArticleDto>> GetNewsByRatingByPageAsync(int page);
        Task<ArticleDto> MostRatedArticleByPeriodOfTime(float? maxCoef);
        Task<float?> MaxCoefOfToday();
        Task<float?> MaxCoefOfTheMonth();
        Task<float?> MaxCoefOfAllTime();
    }
}
