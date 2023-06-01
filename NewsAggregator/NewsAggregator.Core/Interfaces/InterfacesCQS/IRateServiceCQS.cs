using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IRateServiceCQS
    {
        Task<string?> GetCleanTextOfArticle(ArticleDto dto);
        Task<string> CleanTextFromSymbols(string text);
        Task<string?> GetJsonFromTexterra(string? newsText);
        Task<ArticleDto> GetRatingForNews();
        Task<int?> RateArticle();
    }
}
