using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Helpers;
using NUnit.Framework;


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
                Id = Guid.NewGuid()
            };

            await _articleService.CreateAsync(articleDto);

            _unitOfWork.Verify(uOw => uOw.Articles.Add(It.IsAny<Article>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _articleService.CreateAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var articleDto = new CreateOrEditArticleDto
            {
                Id = Guid.NewGuid()
            };

            await _articleService.UpdateAsync(articleDto);

            _unitOfWork.Verify(uOw => uOw.Articles.Update(It.IsAny<Article>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _articleService.UpdateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new Article { Id = id };

            _unitOfWork.Setup(uOw => uOw.Articles.GetById(id))
                .ReturnsAsync(expected);

            await _articleService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Articles.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _articleService.DeleteAsync(It.IsAny<Guid>()));
        }

        [Test]
        public async Task GetAllExistingArticleUrls_CorrectlyReturnsListOfUrls()
        {
            var urls = await _articleService.GetAllExistingArticleUrls();

            Assert.NotNull(urls);
            Assert.AreEqual(30, urls.Count());
        }

        [Test]
        public async Task GetAllExistingArticleUrls_WithNoArticlesFromDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Article>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Articles.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _articleService.GetAllExistingArticleUrls());
        }

        [Test]
        public async Task GetArticleWithoutRating_CorrectlyReturnedOneArticleWithNullCoefficient()
        {
            var article = await _articleService.GetArticleWithoutRating();

            Assert.NotNull(article);
        }

        [Test]
        [TestCase(5.0f)]
        [TestCase(4.1f)]
        [TestCase(4.8f)]
        public async Task MostRatedArticleByPeriodOfTime_WithExistingCoefficient_CorrectlyReturndeArticleWithThatRating(float? coefficient)
        {
            var article = await _articleService.MostRatedArticleByPeriodOfTime(coefficient);

            Assert.NotNull(article);
            Assert.AreEqual(coefficient, article.Coefficient);
        }

        [Test]
        [TestCase(6.0f)]
        [TestCase(7.1f)]
        [TestCase(-13f)]
        public async Task MostRatedArticleByPeriodOfTime_WithNotExistingCoefficient_ReturnedNull(float? coefficient)
        {
            var article = await _articleService.MostRatedArticleByPeriodOfTime(coefficient);

            Assert.Null(article);
        }

        [Test]
        public async Task MaxCoefOfToday_CorrectlyReturnedMaxCoefficientOfToday()
        {
            var coef = await _articleService.MaxCoefOfToday();

            Assert.NotNull(coef);
            Assert.AreEqual(5.0f, coef);
        }

        [Test]
        public async Task MaxCoefOfTheMonth_CorrectlyReturnedMaxCoefficientOfTheMonth()
        {
            var coef = await _articleService.MaxCoefOfTheMonth();

            Assert.NotNull(coef);
            Assert.AreEqual(5.0f, coef);
        }

        [Test]
        public async Task MaxCoefOfAllTime_CorrectlyReturnedMaxCoefficient()
        {
            var coef = await _articleService.MaxCoefOfAllTime();

            Assert.NotNull(coef);
            Assert.AreEqual(5.0f, coef);
        }
    }
}
