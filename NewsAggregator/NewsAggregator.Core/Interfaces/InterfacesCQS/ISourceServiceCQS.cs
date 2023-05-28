using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ISourceServiceCQS
    {
        Task<IEnumerable<SourceDto>> GetAllSourcesAsync();
        Task<SourceDto> GetSourceAsync(Guid Id);
        Task<Guid> GetSourceByUrl(string url);
        Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync();
        Task<int?> CreateAsync(SourceDto sourceDto);
        Task<int?> UpdateAsync(SourceDto sourceDto);
        Task<int?> DeleteAsync(Guid id);  
    }
}
