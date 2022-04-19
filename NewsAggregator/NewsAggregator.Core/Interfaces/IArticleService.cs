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
        Task<IEnumerable<ArticleDto>> GetNewsByPageAsync(int page);
        Task<CreateOrEditArticleDto> GetArticleAsync(Guid Id);
        Task<int?> CreateAsync(CreateOrEditArticleDto articleDto);
        Task<int?> UpdateAsync(CreateOrEditArticleDto articleDto);
        Task<int?> DeleteAsync(Guid modelId);
        Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id);
        Task<List<string>> GetAllExistingArticleUrls();
        Task<ArticleDto> GetArticleWithoutRating();
    }
}
