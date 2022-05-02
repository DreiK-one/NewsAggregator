using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface IRssService
    {
        IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl);
        Task<bool> GetNewsFromSourcesAsync();
    }
}
