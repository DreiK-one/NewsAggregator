using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAllNewsAsync();
        Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id);
        Task<int?> DeleteAsync(Guid modelId);
        Task<List<string>> GetAllExistingArticleUrls();
    }
}
