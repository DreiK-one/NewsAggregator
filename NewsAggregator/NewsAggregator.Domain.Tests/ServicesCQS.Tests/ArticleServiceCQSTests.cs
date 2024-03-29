using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregator.WebAPI.Mappers;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class ArticleServiceCQSTests
    {
        private IMapper _mapper;
        private ArticleServiceCQS _articleServiceCQS;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<ArticleServiceCQS>> _logger;
        private Mock<IConfiguration> _configuration;

        public ArticleServiceCQSTests()
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
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<ArticleServiceCQS>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _articleServiceCQS = new ArticleServiceCQS(
                _mapper,
                _logger.Object,
                _mediator.Object,
                _configuration.Object);
        }

        #region GetAllArticles tests
        [Test]
        [TestCase(1, "User")]
        [TestCase(13, "Moderator")]
        [TestCase(2, null)]
        public async Task GetAllArticles_PositivePageNumberAndWithNonAdminRoleOrNullRole_ReturnedPositiveNewsByPage(int? page, string? role)
        {
            var expected = new List<ArticleDto>()
            {
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetPositiveArticlesByPageQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.AreEqual(articles.Count(), expected.Count());
        }

        [Test]
        [TestCase(6, "Admin")]
        [TestCase(18, "Admin")]
        public async Task GetAllArticles_PositivePageNumberAndWithAdminRole_ReturnedAllNewsByPage(int? page, string? role)
        {
            var expected = new List<ArticleDto>()
            {
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto(),
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetArticlesByPageQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.AreEqual(expected.Count(), articles.Count());
        }

        [Test]
        [TestCase(0, "Admin")]
        public async Task GetAllArticles_ZeroPageNumberAndWithAdminRole_ReturnedData(int? page, string? role)
        {
            var expected = new ArticleDto[2000];

            _mediator.Setup(m => m.Send(It.IsAny<GetAllArticlesQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.That(articles, Is.Not.Null);
        }

        [Test]
        [TestCase(0, "User")]
        [TestCase(0, "Moderator")]
        [TestCase(0, null)]
        public async Task GetAllArticles_ZeroPageNumberAndWithNonAdminRoleOrNullRole_ReturnedData(int? page, string? role)
        {
            var expected = new ArticleDto[2000];

            _mediator.Setup(m => m.Send(It.IsAny<GetAllPositiveArticlesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.That(articles, Is.Not.Null);
        }

        [Test]
        [TestCase(-1, "User")]
        [TestCase(-15, "Moderator")]
        [TestCase(-3, null)]
        public async Task GetAllArticles_NegativePageNumberAndWithNonAdminRoleOrNullRole_ReturnedData(int? page, string? role)
        {
            var expected = new ArticleDto[2000];

            _mediator.Setup(m => m.Send(It.IsAny<GetAllPositiveArticlesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.That(articles, Is.Not.Null);
        }

        [Test]
        [TestCase(-2, "Admin")]
        [TestCase(-14, "Admin")]
        [TestCase(-5, "Admin")]
        public async Task GetAllArticles_NegativePageNumberAndWithAdminRole_ReturnedAllNews(int? page, string? role)
        {
            var expected = new ArticleDto[2000];

            _mediator.Setup(m => m.Send(It.IsAny<GetAllPositiveArticlesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS.GetAllArticles(page, role);

            Assert.That(articles, Is.Not.Null);
        }
        #endregion

        #region GetArticleById tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetArticleById_ExistingId_CorrectlyReturnedData(Guid id)
        {
            var dto = new ArticleDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetArticleByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var article = await _articleServiceCQS.GetArticleById(id);

            Assert.AreEqual(dto.Id, article.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312")]
        public async Task GetArticleById_NoExistentId_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetArticleByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var article = await _articleServiceCQS.GetArticleById(id);

            Assert.Null(article);
        }
        #endregion 

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_CorrectlyReturnedResult()
        {
            var dto = new CreateOrEditArticleDto()
            {
                Id = Guid.NewGuid()
            };

            _mediator.Setup(m => m.Send(It.IsAny<CreateArticleCommand>(),
            It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<int?>());

            await _articleServiceCQS.CreateAsync(dto);

            _mediator.Verify(m => m.Send(It.IsAny<CreateArticleCommand>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnedFalse()
        {
            var res = await _articleServiceCQS.CreateAsync(null);

            Assert.AreEqual(res, null);
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_CorrectlyReturnedResult()
        {
            var dto = new CreateOrEditArticleDto()
            {
                Id = Guid.NewGuid()
            };

            _mediator.Setup(m => m.Send(It.IsAny<EditArticleCommand>(),
            It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<int?>());

            await _articleServiceCQS.UpdateAsync(dto);

            _mediator.Verify(m => m.Send(It.IsAny<EditArticleCommand>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnedFalse()
        {
            var res = await _articleServiceCQS.UpdateAsync(null);

            Assert.AreEqual(res, null);
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_CorrectlyReturnedResult()
        {
            _mediator.Setup(m => m.Send(It.IsAny<DeleteArticleCommand>(),
            It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<int?>());

            await _articleServiceCQS.DeleteAsync(It.IsAny<Guid>());

            _mediator.Verify(m => m.Send(It.IsAny<DeleteArticleCommand>(), It.IsAny<CancellationToken>()));
        }
        #endregion

        #region GetArticleWithAllNavigationProperties tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetArticleWithAllNavigationProperties_ExistingId_CorrectlyReturnedDto(Guid id)
        {
            var dto = new ArticleDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetArticleWithAllNavigationPropertiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var article = await _articleServiceCQS
                .GetArticleWithAllNavigationProperties(id);

            Assert.AreEqual(dto.Id, article.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312")]
        public async Task GetArticleWithAllNavigationProperties_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetArticleWithAllNavigationPropertiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var article = await _articleServiceCQS
                .GetArticleWithAllNavigationProperties(id);

            Assert.Null(article);
        }
        #endregion 

        #region GetArticleWithAllNavigationPropertiesByRating tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetArticleWithAllNavigationPropertiesByRating_ExistingId_CorrectlyReturnedDto(Guid id)
        {
            var dto = new ArticleDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetArticleWithAllNavigationPropertiesByRatingQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var article = await _articleServiceCQS
                .GetArticleWithAllNavigationPropertiesByRating(id);

            Assert.AreEqual(dto.Id, article.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312")]
        public async Task GetArticleWithAllNavigationPropertiesByRating_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetArticleWithAllNavigationPropertiesByRatingQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var article = await _articleServiceCQS
                .GetArticleWithAllNavigationPropertiesByRating(id);

            Assert.Null(article);
        }
        #endregion 

        #region GetAllExistingArticleUrls tests
        [Test]
        public async Task GetAllExistingArticleUrls_CorrectlyReturnedData()
        {
            var expected = new List<string> 
            { 
                "asada",
                "awertdg",
                "tertert"
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetAllExistingArticleUrlsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var urls = await _articleServiceCQS
                .GetAllExistingArticleUrls();

            Assert.AreEqual(expected.Count(), urls.Count());
        }
        #endregion 

        #region GetArticleWithoutRating tests
        [Test]
        public async Task GetArticleWithoutRating_CorrectlyReturnedDto()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetArticleWithoutRatingQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new ArticleDto());

            var article = await _articleServiceCQS
                .GetArticleWithoutRating();

            Assert.IsInstanceOf<ArticleDto>(article);
        }
        #endregion 

        #region GetAllNewsByRatingAsync tests
        [Test]
        public async Task GetAllNewsByRatingAsync_CorrectlyReturnedData()
        {
            var expected = new List<ArticleDto>
            {
                new ArticleDto(),
                new ArticleDto(),
                new ArticleDto()
            }.AsEnumerable();

            _mediator.Setup(m => m.Send(It.IsAny<GetAllNewsByRatingAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS
                .GetAllNewsByRatingAsync();

            Assert.AreEqual(expected.Count(), articles.Count());
        }
        #endregion 

        #region GetNewsByRatingByPageAsync tests
        [Test]
        [TestCase(1)]
        [TestCase(13)]
        [TestCase(2)]
        public async Task GetNewsByRatingByPageAsync_PositivePageNumber_CorrectlyReturnedDtos(int page)
        {
            var expected = new ArticleDto[2000].AsEnumerable();

            _mediator.Setup(m => m.Send(It.IsAny<GetNewsByRatingByPageAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var articles = await _articleServiceCQS
                .GetNewsByRatingByPageAsync(page);

            Assert.AreEqual(articles.Count(), expected.Count());
        }
        #endregion

        #region MostRatedArticleByPeriodOfTime tests
        [Test]
        [TestCase(0.4f)]
        [TestCase(4.5f)]
        public async Task MostRatedArticleByPeriodOfTime_ExistingCoef_CorrectlyReturnedDto(float coef)
        {
            var dto = new ArticleDto()
            {
                Coefficient = coef
            };

            _mediator.Setup(m => m.Send(It.IsAny<MostRatedArticleByPeriodOfTimeQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var article = await _articleServiceCQS
                .MostRatedArticleByPeriodOfTime(coef);

            Assert.AreEqual(dto.Coefficient, article.Coefficient);
        }

        [Test]
        public async Task MostRatedArticleByPeriodOfTime_ReturnedNull()
        {
            _mediator.Setup(m => m.Send(It.IsAny<MostRatedArticleByPeriodOfTimeQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var article = await _articleServiceCQS
                .MostRatedArticleByPeriodOfTime(null);

            Assert.Null(article);
        }
        #endregion 

        #region MaxCoefOfToday tests
        [Test]
        public async Task MaxCoefOfToday_ExistingCoef_CorrectlyReturnedDto()
        {
            var expected = 4.5f;

            _mediator.Setup(m => m.Send(It.IsAny<MaxCoefOfTodayQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var coef = await _articleServiceCQS
                .MaxCoefOfToday();

            Assert.IsInstanceOf<float?>(coef);
        }

        [Test]
        public async Task MaxCoefOfToday_NoData_ReturnedNull()
        {
            var coef = await _articleServiceCQS
                .MaxCoefOfToday();

            Assert.Null(coef);
        }
        #endregion

        #region MaxCoefOfTheMonth tests
        [Test]
        public async Task MaxCoefOfTheMonth_ExistingCoef_CorrectlyReturnedDto()
        {
            var expected = 4.5f;

            _mediator.Setup(m => m.Send(It.IsAny<MaxCoefOfTheMonthQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var coef = await _articleServiceCQS
                .MaxCoefOfTheMonth();

            Assert.IsInstanceOf<float?>(coef);
        }

        [Test]
        public async Task MaxCoefOfTheMonth_NoData_ReturnedNull()
        {
            var coef = await _articleServiceCQS
                .MaxCoefOfTheMonth();

            Assert.Null(coef);
        }
        #endregion

        #region MaxCoefOfAllTime tests
        [Test]
        public async Task MaxCoefOfAllTime_ExistingCoef_CorrectlyReturnedDto()
        {
            var expected = 4.5f;

            _mediator.Setup(m => m.Send(It.IsAny<MaxCoefOfAllTimeQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var coef = await _articleServiceCQS
                .MaxCoefOfAllTime();

            Assert.IsInstanceOf<float?>(coef);
        }

        [Test]
        public async Task MaxCoefOfAllTime_NoData_ReturnedNull()
        {
            var coef = await _articleServiceCQS
                .MaxCoefOfAllTime();

            Assert.Null(coef);
        }
        #endregion
    }
}