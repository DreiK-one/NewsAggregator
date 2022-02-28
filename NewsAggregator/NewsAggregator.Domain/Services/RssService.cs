using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NewsAggregator.Domain.Services
{
    public class RssService : IRssService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RssService> _logger;
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IHtmlParserService _htmlParserService;

        public RssService(IMapper mapper, 
            ILogger<RssService> logger,
            IArticleService articleService,
            ISourceService sourceService,
            IHtmlParserService htmlParserService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _sourceService = sourceService;
            _htmlParserService = htmlParserService;
        }

        public IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl)
        {
            try
            {
                using (var reader = XmlReader.Create(rssUrl))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    var result = feed.Items.Select(item => _mapper.Map<RssArticleDto>(item)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }            
        }

        public async Task<bool> GetNewsFromSources()
        {
            try
            {
                var rssUrls = await _sourceService.GetRssUrlsAsync();
                var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto?>();
                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    GetArticlesInfoFromRss(dto.RssUrl).AsParallel().ForAll(articleDto
                        => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
                });

                var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

                Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                    s => concurrentDictionary.Remove(s, out var dto));

                foreach (var rssArticleDto in concurrentDictionary)
                {
                    var articleInfo = await _htmlParserService.GetArticleContentFromUrlAsync(rssArticleDto.Key);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
