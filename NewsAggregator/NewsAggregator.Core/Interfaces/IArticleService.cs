using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Specifications;
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
        Task<ArticleDto> GetArticleWithAllNavigationPropertiesByRating(Guid id);
        Task<List<string>> GetAllExistingArticleUrls();
        Task<ArticleDto> GetArticleWithoutRating();
        Task<IEnumerable<ArticleDto>> GetAllNewsByRatingAsync();
        Task<IEnumerable<ArticleDto>> GetNewsByRatingByPageAsync(int page);
        Task<ArticleDto> MostRatedArticleByPeriodOfTime(float? maxCoef);

        Task<float?> MaxCoefOfToday();
        Task<float?> MaxCoefOfTheMonth();
        Task<float?> MaxCoefOfAllTime();


        //Task<List<RequestArticleDto>> GetArticlesByRequest(Specification<RequestArticleDto> spec);
    }
}
