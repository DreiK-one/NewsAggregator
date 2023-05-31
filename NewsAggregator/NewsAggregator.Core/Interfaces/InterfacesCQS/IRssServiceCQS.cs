using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IRssServiceCQS
    {
        IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl);
        Task<bool> GetNewsFromSourcesAsync();
    }
}
