using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Helpers;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<ArticleService>> _logger;
        private Mock<IConfiguration> _configuration;
        private ArticleService _articleService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _logger= new Mock<ILogger<ArticleService>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .Returns(TestFunctions.GetDbSet(TestArticlesData.Articles).Object);

            _articleService = new ArticleService(
                _mapper.Object, 
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
        public async Task GetAllNewsAsync_ReturnedAnyoneException()
        {
            var nullList = new List<Article>().AsQueryable();
            _unitOfWork.Setup(uOw => uOw.Articles.Get()).Returns(nullList);

            Assert.ThrowsAsync<InvalidOperationException>(() => _articleService.GetAllNewsAsync());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(0)]
        public async Task GetNewsByPageAsync_WithRightPage_CorrectlyReturnsArticles(int page)
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
            _mapper.Setup(mapper => mapper.Map<CreateOrEditArticleDto>(expected))
                .Returns(new CreateOrEditArticleDto { Id = id });

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
            var articles = _articleService.GetArticleWithAllNavigationProperties(id);
        }
    }
}
