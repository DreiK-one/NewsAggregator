﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;
using System.Collections.Concurrent;
using System.ServiceModel.Syndication;
using System.Xml;


namespace NewsAggregator.Domain.Services
{
    public class RssService : IRssService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RssService> _logger;
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ICategoryService _categoryService;

        public RssService(IMapper mapper,
            ILogger<RssService> logger,
            IArticleService articleService,
            ISourceService sourceService,
            ICategoryService categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _sourceService = sourceService;
            _categoryService = categoryService;
        }

        public IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl)
        {
            try
            {
                using (var reader = XmlReader.Create(rssUrl))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    var result = feed.Items
                        .Select(item => _mapper.Map<RssArticleDto>(item)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }            
        }

        public async Task<bool> GetNewsFromSourcesAsync()
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
                    var articleUrl = await _articleService.CreateAsync(
                        new CreateOrEditArticleDto
                        {
                            Id = Guid.NewGuid(),
                            Title = " ",
                            Body = " ",
                            Description = " ",
                            SourceUrl = rssArticleDto.Key,
                            Image = " ",
                            Coefficient = null,
                            CreationDate = DateTime.Now,
                            CategoryId = await _categoryService.GetCategoryByUrl(rssArticleDto.Key),
                            SourceId = await _sourceService.GetSourceByUrl(rssArticleDto.Key),
                        });
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
