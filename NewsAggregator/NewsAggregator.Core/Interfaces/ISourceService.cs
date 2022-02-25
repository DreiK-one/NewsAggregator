using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
