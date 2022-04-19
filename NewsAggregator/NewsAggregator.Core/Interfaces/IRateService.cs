using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IRateService
    {
        Task<string?> GetCleanTextOfArticle();
        string CleanTextFromSymbols(string text);
        Task<string?> GetJsonFromTexterra();
        Task<float?> GetRatingForNews();
    }
}
