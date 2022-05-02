using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceDto>> GetAllSourcesAsync();
        Task<int?> CreateAsync(SourceDto sourceDto);
        Task<int?> UpdateAsync(SourceDto sourceDto);
        Task<int?> DeleteAsync(Guid id);
        Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync();
        Task<Guid> GetSourceByUrl(string url);
        Task<SourceDto> GetSourceAsync(Guid Id);
    }
}
