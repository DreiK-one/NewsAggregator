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
        Task<int?> CreateAsync(CreateArticleDto articleDto);
        Task<int?> UpdateAsync(ArticleDto articleDto);
        Task<int?> DeleteAsync(Guid modelId);
        Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id);
        Task<List<string>> GetAllExistingArticleUrls();
    }
}
