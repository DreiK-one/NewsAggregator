using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using System.Collections.Concurrent;
using System.ServiceModel.Syndication;
using System.Xml;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class RssServiceCQS : IRssServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RssServiceCQS> _logger;
        private readonly IArticleServiceCQS _articleServiceCQS;
        private readonly ISourceServiceCQS _sourceServiceCQS;
        private readonly ICategoryServiceCQS _categoryServiceCQS;

        public RssServiceCQS(IMapper mapper,
            ILogger<RssServiceCQS> logger,
            IArticleServiceCQS articleService,
            ISourceServiceCQS sourceService,
            ICategoryServiceCQS categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleServiceCQS = articleService;
            _sourceServiceCQS = sourceService;
            _categoryServiceCQS = categoryService;
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
                var rssUrls = await _sourceServiceCQS.GetRssUrlsAsync();
                var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto?>();
                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    GetArticlesInfoFromRss(dto.RssUrl).AsParallel().ForAll(articleDto
                        => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
                });

                var extArticlesUrls = await _articleServiceCQS.GetAllExistingArticleUrls();

                Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                    s => concurrentDictionary.Remove(s, out var dto));

                foreach (var rssArticleDto in concurrentDictionary)
                {
                    var articleUrl = await _articleServiceCQS.CreateAsync(
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
                            CategoryId = await _categoryServiceCQS.GetCategoryByUrlAsync(rssArticleDto.Key),
                            SourceId = await _sourceServiceCQS.GetSourceByUrl(rssArticleDto.Key),
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
