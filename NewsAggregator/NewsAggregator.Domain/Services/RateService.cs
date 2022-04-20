using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Domain.Services.DeserializationEntities;
using Newtonsoft.Json;
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

        public async Task<ArticleDto> GetRatingForNews()
        {
            try
            {
                var article = await _articleService.GetArticleWithoutRating();

                var cleantext = await GetCleanTextOfArticle(article);
                var unratedArticle = await GetJsonFromTexterra(cleantext);
                var fileWithRatedWords = File.ReadAllText("Words.json");

                Dictionary<string, int?> ratedWordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, int?>>(fileWithRatedWords);
                Root deserializedArticles = JsonConvert.DeserializeObject<Root>(unratedArticle);

                int summuryRating = 0;
                float count = 0;

                foreach (var unratedword in deserializedArticles.Annotations.Lemma)
                {
                    if (ratedWordsDictionary.ContainsKey(unratedword.Value) && unratedword.Value != "")
                    {
                        summuryRating += ratedWordsDictionary[unratedword.Value].Value;
                        count++;
                    }
                }

                var rating = summuryRating / (count == 0 ? 1f : count);

                var ratedArticle = new ArticleDto
                {
                    Id = article.Id,
                    Coefficient = rating
                };

                return ratedArticle; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string?> GetCleanTextOfArticle(ArticleDto dto)
        {
            try
            {
                var body = await CleanTextFromSymbols(dto.Body);
                var descrtiption = await CleanTextFromSymbols(dto.Description);
                var title = await CleanTextFromSymbols(dto.Title);

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

        public async Task<string> CleanTextFromSymbols(string text)
        {
            try
            {
                var result = text.Replace("<p>", "").Replace("</p>", "")
                    .Replace("<em>", "").Replace("</em>", "")
                    .Replace("&quot;", "").Replace("\"", "")
                    .Replace("«", "").Replace("»", "").Replace("\n", "");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string?> GetJsonFromTexterra(string? newsText)
        {
            try
            {
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
         
                    var result = responseString.Substring(1, responseString.Length - 2);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> RateArticle()
        {
            try
            {
                var ratedArticle = GetRatingForNews().Result;
                if (ratedArticle != null)
                {
                    await _unitOfWork.Articles.PatchAsync(ratedArticle.Id, new List<PatchModel>
                    {
                        new PatchModel
                        {
                            PropertyName = "Coefficient",
                            PropertyValue = ratedArticle.Coefficient
                        }
                    });
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
