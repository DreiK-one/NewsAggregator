using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class ArticleServiceCQSTests
    {
        private ArticleServiceCQS _articleServiceCQS;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<ArticleServiceCQS>> _logger;
        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<ArticleServiceCQS>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _articleServiceCQS = new ArticleServiceCQS(
                _logger.Object,
                _mediator.Object,
                _configuration.Object);
        }

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
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312")] //fake id
        public async Task GetArticleById_NoExistentId_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetArticleByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var article = await _articleServiceCQS.GetArticleById(id);

            Assert.Null(article);
        }

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
    }
}