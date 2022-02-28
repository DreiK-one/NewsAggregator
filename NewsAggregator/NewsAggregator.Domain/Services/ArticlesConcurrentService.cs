using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class ArticlesConcurrentService : IArticlesConcurrentService
    {
        private readonly ILogger<ArticlesConcurrentService> _logger;
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly IHtmlParserService _htmlParserService;

        public ArticlesConcurrentService(ILogger<ArticlesConcurrentService> logger, 
            IArticleService articleService, 
            ISourceService sourceService, 
            IRssService rssService, 
            IHtmlParserService htmlParserService)
        {
            _logger = logger;
            _articleService = articleService;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
        }

        public async Task<int?> GetNewsFromSources()
        {
            try
            {
                var rssUrls = await _sourceService.GetRssUrlsAsync();
                var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto?>();
                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    _rssService.GetArticlesInfoFromRss(dto.RssUrl).AsParallel().ForAll(articleDto
                        => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
                });

                var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

                Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                    s => concurrentDictionary.Remove(s, out var dto));

                foreach (var rssArticleDto in concurrentDictionary)
                {
                    var articleInfo = await _htmlParserService.GetArticleContentFromUrlAsync(rssArticleDto.Key);
                }
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            } 
        }
    }
}
