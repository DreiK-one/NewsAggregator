using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IHtmlParserServiceCQS
    {
        Task<int?> GetArticleContentFromUrlAsync();
        Task<NewArticleDto> ParseOnlinerArticle(string url);
        Task<NewArticleDto> ParseGohaArticle(string url);
        Task<NewArticleDto> ParseShazooArticle(string url);
    }
}
