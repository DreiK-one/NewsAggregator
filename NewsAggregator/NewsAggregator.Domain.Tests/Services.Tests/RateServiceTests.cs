using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using NewsAggregator.Core.Interfaces;
using System.Threading.Tasks;
using System;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System.Collections.Generic;

namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class RateServiceTests
    {
        private Mock<ILogger<RateService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IArticleService> _articleService;
        private RateService _rateService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<RateService>>();
            _articleService = new Mock<IArticleService>();

            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestArticlesData.Articles).Object);

            _rateService = new RateService(_logger.Object,
                _unitOfWork.Object,
                _articleService.Object);
        }

        #region GetRatingForNews tests
        [Test]
        public async Task GetRatingForNews_CorrectlyReturnsDto()
        {
            var dto = new ArticleDto
            {
                Id = Guid.NewGuid(),
                Body = "<p>Сегодня около 16:30 на пересечении проспекта Дзержинского с улицей Алибегова столкнулись шесть автомобилей: Lada, Toyota, Volkswagen, Acura, BMW, Volkswagen. Предварительно, ДТП произошло по причине несоблюдения безопасной дистанции, сообщает ГАИ Минска.</p>",
                Description = "<br>«Паровоз» из шести автомобилей образовался на столичном перекрестке<br/>",
                Title = "«Паровоз»"
            };

            _articleService.Setup(aS => aS.GetArticleWithoutRating()).ReturnsAsync(dto);

            var res = await _rateService.GetRatingForNews();

            Assert.NotNull(res);
            Assert.AreEqual(res.Coefficient, 0,5);
        }

        [Test]
        public async Task GetRatingForNews_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _rateService.GetRatingForNews());
        }
        #endregion

        #region GetCleanTextOfArticle tests
        [Test]
        public async Task GetCleanTextOfArticle_WithCorrectModel_CorrectlyReturnsResult()
        {
            var dto = new ArticleDto
            {
                Body = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>",
                Description = "«Risus sed vulputate odio ut enim blandit volutpat.»",
                Title = "<br>Lorem ipsum<br/>"
            };

            var expected = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Risus sed vulputate odio ut enim blandit volutpat. Lorem ipsum";

            var res = await _rateService.GetCleanTextOfArticle(dto);

            Assert.NotNull(res);
            Assert.AreEqual(res, expected);
        }

        [Test]
        public async Task GetCleanTextOfArticle_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _rateService.GetCleanTextOfArticle(null));
        }
        #endregion

        #region CleanTextFromSymbols tests
        [Test]
        [TestCase("<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry.</p>")]
        [TestCase("<p>It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.</p>")]
        public async Task CleanTextFromSymbols_CorrectlyReturnsResult(string text)
        {
            var res = await _rateService.CleanTextFromSymbols(text);

            var expected = text.Replace("<p>", "").Replace("</p>", "");

            Assert.NotNull(res);
            Assert.AreEqual(res, expected);
        }
        #endregion

        #region GetJsonFromTexterra tests
        [Test]
        [TestCase("Широко известный в узких кругах спидраннер под ником Distortion2 поставил очередной рекорд в категории Elden Ring Any% Unrestricted")]
        public async Task GetJsonFromTexterra_WithCleanString_CorrectlyReturnsResult(string text)
        {
            var res = await _rateService.GetJsonFromTexterra(text);

            Assert.NotNull(res);
        }

        [Test]
        [TestCase("<p>Широко известный в узких кругах спидраннер под ником Distortion2 поставил очередной рекорд в категории Elden Ring Any% Unrestricted. With \" simple\"</p>")]
        public async Task GetJsonFromTexterra_WithTagsString_ReturnsArgumentOutOfRangeException(string text)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
                await _rateService.GetJsonFromTexterra(text));
        }
        #endregion

        #region RateArticle tests
        [Test]
        public async Task RateArticle_ReturnsSaveResult()
        {
            var dto = new ArticleDto
            {
                Id = Guid.NewGuid(),
                Body = "<p>Сегодня около 16:30 на пересечении проспекта Дзержинского с улицей Алибегова столкнулись шесть автомобилей: Lada, Toyota, Volkswagen, Acura, BMW, Volkswagen. Предварительно, ДТП произошло по причине несоблюдения безопасной дистанции, сообщает ГАИ Минска.</p>",
                Description = "<br>«Паровоз» из шести автомобилей образовался на столичном перекрестке<br/>",
                Title = "«Паровоз»"
            };

            _articleService.Setup(aS => aS.GetArticleWithoutRating()).ReturnsAsync(dto);

            await _rateService.RateArticle();

            _unitOfWork.Verify(uOw => uOw.Articles.PatchAsync(dto.Id, It.IsAny<List<PatchModel>>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task RateArticle_WithNullModel_ReturnsAggregateException()
        {
            Assert.ThrowsAsync<AggregateException>(async () =>
                await _rateService.RateArticle());
        }
        #endregion
    }
}
