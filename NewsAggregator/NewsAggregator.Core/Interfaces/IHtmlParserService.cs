using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface IHtmlParserService
    {
        Task<int?> GetArticleContentFromUrlAsync(string url);
        Task<NewArticleDto> ParseOnlinerArticle(string url);
        Task<NewArticleDto> ParseGohaArticle(string url);
        Task<NewArticleDto> ParseShazooArticle(string url);
    }
}
