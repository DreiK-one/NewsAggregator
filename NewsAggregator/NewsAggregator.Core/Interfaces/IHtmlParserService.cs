using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IHtmlParserService
    {
        Task<int?> GetArticleContentFromUrlAsync(string url);
        Task<NewArticleDto> ParseOnlinerArticle(string url);
        Task<NewArticleDto> ParseRiaArticle(string url);
        Task<NewArticleDto> ParseShazooArticle(string url);
    }
}
