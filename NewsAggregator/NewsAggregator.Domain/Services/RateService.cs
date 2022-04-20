﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
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

        public async Task<string?> GetCleanTextOfArticle()
        {
            try
            {
                var article = await _articleService.GetArticleWithoutRating();

                var body = CleanTextFromSymbols(article.Body);
                var descrtiption = CleanTextFromSymbols(article.Description);
                var title = CleanTextFromSymbols(article.Title);

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

        public string CleanTextFromSymbols(string text)
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

        public async Task<float?> GetRatingForNews()
        {
            var unratedArticle = await GetJsonFromTexterra();
            var fileWithRatedWords = File.ReadAllText("Words.json");

            Dictionary<string, int?> ratedWordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, int?>>(fileWithRatedWords);
            Root deserializedArticles = JsonConvert.DeserializeObject<Root>(unratedArticle);

            int? rating = 0;
            float? count = deserializedArticles.Annotations.Lemma.Count;

            foreach (var unratedword in deserializedArticles.Annotations.Lemma)
            {
                if(ratedWordsDictionary.ContainsKey(unratedword.Value) && unratedword.Value != "")
                {
                    rating += ratedWordsDictionary[unratedword.Value].Value;
                }
            }   
            var result = (rating / count);

            return result;
        }
    }
}