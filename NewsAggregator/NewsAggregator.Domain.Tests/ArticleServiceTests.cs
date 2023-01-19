using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;


namespace NewsAggregator.Domain.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;
        private Mock<ILogger<ArticleService>> _logger;
        private Mock<IConfiguration> _configuration;
        private ArticleService _articleService;

        public ArticleServiceTests()
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
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger= new Mock<ILogger<ArticleService>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .ReturnsAsync(TestFunctions.GetArticles(TestArticlesData.Articles).Object);

            _articleService = new ArticleService(
                _mapper, 
                _logger.Object, 
                _unitOfWork.Object, 
                _configuration.Object);
        }

        [Test]
        public async Task GetAllNewsAsync_CorrectlyReturnedListOfArticles()
        {
            var articles = await _articleService.GetAllNewsAsync();

            Assert.IsNotNull(articles);
            Assert.AreEqual(30, articles.Count());
        }

        [Test]
        public async Task GetAllNewsAsync_WithNoArticlesFromDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Article>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Articles.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _articleService.GetAllNewsAsync());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(0)]
        public async Task GetNewsByPageAsync_WithExistingPage_CorrectlyReturnsArticles(int page)
        {
            var articles = await _articleService.GetNewsByPageAsync(page);

            Assert.IsNotNull(articles);
            Assert.AreEqual(10, articles.Count());
        }

        [Test]
        public async Task GetAllNewsByRatingAsync_ReturnedListOfArticlesWithPositiveRating()
        {
            var articles = await _articleService.GetAllNewsByRatingAsync();

            Assert.IsNotNull(articles);
            Assert.AreEqual(13, articles.Count());
        }

        [Test]
        [TestCase(0, 10)]
        [TestCase(1, 3)]
        [TestCase(2, 0)]
        public async Task GetNewsByRatingByPageAsync_CorrectlyReturnedArticles(int page, int expected)
        {
            var articles = await _articleService.GetNewsByRatingByPageAsync(page);

            Assert.AreEqual(expected, articles.Count());
        }

        [Test]
        [TestCase("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c")]
        [TestCase("e076479a-a96f-40fe-a54f-ddaceec29558")]
        public async Task GetArticleAsync_WithExistingId_ReturnsArticle(Guid id)
        {
            var expected = new Article { Id = id };

            _unitOfWork.Setup(uOw => uOw.Articles.GetById(id))
                .ReturnsAsync(expected);

            var article = await _articleService.GetArticleAsync(id);

            Assert.AreEqual(expected.Id, article.Id);
        }

        [Test]
        [TestCase("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c")]
        [TestCase("e076479a-a96f-40fe-a54f-ddaceec29558")]
        public async Task GetArticleAsync_WithNotExistingId_ReturnsNull(Guid id)
        {
            var article = await _articleService.GetArticleAsync(id);

            Assert.Null(article);
        }

        [Test]
        [TestCase("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c")]
        [TestCase("e076479a-a96f-40fe-a54f-ddaceec29558")]
        public async Task GetArticleWithAllNavigationProperties_WithExistingId_ReturnsArticle(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationProperties(id);

            Assert.AreEqual(id, article.Id);
        }

        [Test]
        [TestCase("a3470938-60ce-4ae2-880c-93d39503ddb7")]
        [TestCase("06d0ef95-35b8-40a6-ba02-e8ad99a5fa8c")] 
        public async Task GetArticleWithAllNavigationProperties_WithNonExistingId_ReturnsNull(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationProperties(id);

            Assert.Null(article);
        }

        [Test]
        [TestCase("1ca88641-9f96-417c-ac2f-1c5f343a4080")]
        [TestCase("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c")]
        public async Task GetArticleWithAllNavigationPropertiesByRating_WithExistingId_ReturnsArticleWithPositiveRating(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationPropertiesByRating(id);

            Assert.AreEqual(id, article.Id);
        }

        [Test]
        [TestCase("8609aaed-75d8-4a39-933c-f09cc2694bcc")]
        [TestCase("0a59bc93-6641-4bc8-a116-a33e67ff458d")]
        public async Task GetArticleWithAllNavigationPropertiesByRating_WithNonExistingId_ReturnsNull(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationPropertiesByRating(id);

            Assert.Null(article);
        }

        [Test]
        [TestCase("97f7d7fd-3d77-498d-a024-16c0354b6ccb")]
        [TestCase("e076479a-a96f-40fe-a54f-ddaceec29558")]
        public async Task GetArticleWithAllNavigationPropertiesByRating_WithExistingIdAndNegativeRating_ReturnsNull(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationPropertiesByRating(id);

            Assert.Null(article);
        }

        [Test]
        public async Task CreateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var articleDto = new CreateOrEditArticleDto
            {
                Id = Guid.NewGuid(),
                Title = "test",
                Description = "test",
                Body = "test",
                SourceUrl = "test",
                Image = "test",
                CreationDate = DateTime.Now,
                CategoryId = Guid.NewGuid(),
                SourceId = Guid.NewGuid()
            };

            var testData = new List<Article>();

            var mockData = TestFunctions.GetDbSet(testData.AsQueryable());
            mockData.Setup(m => m.AddAsync(It.IsAny<Article>(), default)).Callback<Article, CancellationToken>((s, token) =>
            {
                testData.Add(s);
            });

            var mockContext = new Mock<NewsAggregatorContext>();
            mockContext.Object.Articles = mockData.Object;

            


            var result = await _articleService.CreateAsync(articleDto);

            _unitOfWork.Verify(uOw => uOw.Articles.Add(_mapper.Map<Article>(articleDto)), Times.Once);
            _unitOfWork.Verify(uOw => uOw.Save(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithNulltModel_ReturnsNullReferenceException()
        {
            var result = _articleService.CreateAsync(null);

            Assert.ThrowsAsync<NullReferenceException>(async () => await result);
        }
    }
}
