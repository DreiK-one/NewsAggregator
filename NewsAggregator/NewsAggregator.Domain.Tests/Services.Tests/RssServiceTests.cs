using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using NewsAggregator.Data.Entities;
using System.Collections.Generic;
using System;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System.Xml;

namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class RssServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<RssService>> _logger;
        private Mock<IArticleService> _articleService;
        private Mock<ISourceService> _sourceService;
        private Mock<ICategoryService> _categoryService;
        private RssService _rssService;

        public RssServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ArticleProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<RssService>>();
            _articleService = new Mock<IArticleService>();
            _sourceService = new Mock<ISourceService>();
            _categoryService = new Mock<ICategoryService>();

            _rssService = new RssService(_mapper,
                _logger.Object,
                _articleService.Object,
                _sourceService.Object,
                _categoryService.Object);
        }

        #region GetArticlesInfoFromRss tests
        [Test]
        [TestCase("https://www.onliner.by/feed")]
        [TestCase("https://www.goha.ru/rss/articles")]
        public void GetArticlesInfoFromRss_WithCorrectUrl_ReturnsModel(string rssUrl)
        {
            var result = _rssService.GetArticlesInfoFromRss(rssUrl);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("https://www.onliner.by/")]
        [TestCase("https://www.goha.ru/")]
        public void GetArticlesInfoFromRss_WithIncorrectUrl_ReturnsXmlException(string rssUrl)
        {
            Assert.Throws<XmlException>(() => _rssService.GetArticlesInfoFromRss(rssUrl));
        }
        #endregion

        #region GetNewsFromSourcesAsync tests
        [Test]
        public async Task GetNewsFromSourcesAsync_WorkCorrect()
        {
            var sources = new List<RssUrlsFromSourceDto>
            {
                new RssUrlsFromSourceDto
                {
                    SourceId = Guid.NewGuid(),
                    RssUrl = "http://onliner.by/feed"
                },
                new RssUrlsFromSourceDto
                {
                    SourceId = Guid.NewGuid(),
                    RssUrl = "https://www.goha.ru/rss/articles"
                }
            }.AsEnumerable();

            var articleUrls = new List<string>
            {
                "https://www.goha.ru/csgo-ne-uderzhat-pikovyj-onlajn-perevalil-za-18-mln-lamrDO",
                "https://www.goha.ru/igrovoj-process-marvels-midnight-suns-za-grozu-iz-lyudej-iks-lj6o9N",
                "https://auto.onliner.by/2023/05/07/retro-minsk-2023"
            };

            _sourceService.Setup(sS => sS.GetRssUrlsAsync()).ReturnsAsync(sources);
            _articleService.Setup(aS => aS.GetAllExistingArticleUrls()).ReturnsAsync(articleUrls);
            _categoryService.Setup(cS => cS.GetCategoryByUrl(It.IsAny<string>())).Returns(Task.Run(() => Guid.NewGuid()));
            _sourceService.Setup(sS => sS.GetSourceByUrl(It.IsAny<string>())).Returns(Task.Run(() => Guid.NewGuid()));

            var result = await _rssService.GetNewsFromSourcesAsync();

            _articleService.Verify(aS => aS.CreateAsync(It.IsAny<CreateOrEditArticleDto>()));
            _categoryService.Verify(cS => cS.GetCategoryByUrl(It.IsAny<string>()));
            _sourceService.Verify(sS => sS.GetSourceByUrl(It.IsAny<string>()));
        }
        #endregion
    }
}
