using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class RateService : IRateService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RateService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleService _articleService;

        public RateService(IMapper mapper,
            ILogger<RateService> logger,
            IUnitOfWork unitOfWork, 
            IArticleService articleService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _articleService = articleService;
        }

        public async Task<string?> GetCleanTextOfArticle()
        {
            try
            {
                var article = await _articleService.GetArticleWithoutRating();

                var body = CleanText(article.Body);
                var descrtiption = CleanText(article.Description);
                var title = CleanText(article.Title);

                var fullText = string.Concat(body, " ", descrtiption, " ", title);

                var result = Regex.Replace(fullText, "(<[^>]+>)", "");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }  
        }

        public string CleanText(string text)
        {
            var result = text.Replace("<p>", "").Replace("</p>", "")
                    .Replace("<em>", "").Replace("</em>", "")
                    .Replace("&quot;", "").Replace("\"", "")
                    .Replace("«", "").Replace("»", "").Replace("\n", "");
            return result;
        }

        public async Task<string?> GetJsonFromTexterra()
        {
            try
            {
                var newsText = await GetCleanTextOfArticle();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var request = new HttpRequestMessage(HttpMethod.Post, "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=bc1bfe69945f1cc9f1b565b0928f537065d21b25")
                    {
                        Content = new StringContent("[{\"text\":\"" + newsText + "\"}]",

                            Encoding.UTF8,
                            "application/json")
                    };
                    var response = await httpClient.SendAsync(request);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<float?> GetRatingForNews()
        {
            throw new NotImplementedException();
        }
    }
}
