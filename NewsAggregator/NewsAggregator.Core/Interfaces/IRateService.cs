using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IRateService
    {
        Task<string?> GetCleanTextOfArticle(ArticleDto dto);
        Task<string> CleanTextFromSymbols(string text);
        Task<string?> GetJsonFromTexterra(string? newsText);
        Task<ArticleDto> GetRatingForNews();
    }
}
