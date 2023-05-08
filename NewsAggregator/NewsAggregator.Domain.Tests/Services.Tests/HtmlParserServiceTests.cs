using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using NewsAggregator.Core.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class HtmlParserServiceTests
    {
        private Mock<ILogger<HtmlParserService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISourceService> _sourceService;
        private HtmlParserService _htmlParserService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<HtmlParserService>>();
            _sourceService = new Mock<ISourceService>();

            _htmlParserService = new HtmlParserService(_logger.Object,
                _unitOfWork.Object,
                _sourceService.Object);
        }

        #region GetArticleContentFromUrlAsync tests
        [Test]
        [TestCase("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c", "F2FB2A60-C1DE-4DA5-B047-0871D2D677B5")]
        [TestCase("e076479a-a96f-40fe-a54f-ddaceec29558", "F2FB2A60-C1DE-4DA5-B047-0871D2D677B4")]
        [TestCase("16c80b2d-db21-4b0f-aeb1-c2bc5e925e9a", "C13088A4-9467-4FCE-9EF7-3903425F1F81")]
        public async Task GetArticleContentFromUrlAsync_WithCorrectIds_ReturnsSaveResult(string articleid, string sourceId)
        {
            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestHtmlParserData.Articles
                    .Where(s => s.Id == new Guid(articleid))).Object);

            _sourceService.Setup(sS => sS.GetSourceByUrl(It.IsAny<string>()))
                .Returns(Task.Run(() => new Guid(sourceId)));

            await _htmlParserService.GetArticleContentFromUrlAsync();

            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task GetArticleContentFromUrlAsync_WithIncorrectArticleId_ReturnsNull()
        {
            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestHtmlParserData.Articles
                    .Where(s => s.Id == new Guid())).Object);

            var res = await _htmlParserService.GetArticleContentFromUrlAsync();

            Assert.Null(res);
        }

        [Test]
        public async Task GetArticleContentFromUrlAsync_WithIncorrectSourceId_ReturnsNull()
        {
            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestHtmlParserData.Articles
                    .Where(s => s.Id == new Guid("49139cd4-6761-4eaf-a5ec-eb212ee7ee0c"))).Object);

            _sourceService.Setup(sS => sS.GetSourceByUrl(It.IsAny<string>()))
                .Returns(Task.Run(() => new Guid()));

            var res = await _htmlParserService.GetArticleContentFromUrlAsync();

            Assert.Null(res);
        }
        #endregion

        #region ParseOnlinerArticle tests
        [Test]
        [TestCase("https://auto.onliner.by/2023/02/24/v-minske-mogut-zapretit-parkovku-na-ulicax-po-opredelennym-dnyam-zimoj")]
        [TestCase("https://tech.onliner.by/2022/04/21/minyust-podreguliroval-izyatie-kriptovalyuty-u-dolzhnikov")]
        public async Task ParseOnlinerArticle_WithCorrectUrl_CorrectlyReturnsModel(string url)
        {
            var articleDto = await _htmlParserService.ParseOnlinerArticle(url);

            Assert.NotNull(articleDto);
        }

        [Test]
        [TestCase("https://www.goha.ru/v-ekranizacii-fallout-snimetsya-ella-pernell-dzhinks-iz-arkejn-lO1vLE")]
        [TestCase("https://www.goha.ru/elden-ring-projdena-menee-chem-za-9-minut-4AqYeg")]
        public async Task ParseOnlinerArticle_WithIncorrectUrl_ReturnsNullReferenceException(string url)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _htmlParserService.ParseOnlinerArticle(url));
        }
        #endregion

        #region ParseGohaArticle tests
        [Test]
        [TestCase("https://www.goha.ru/ilon-mask-sobralsya-vzhivit-sebe-chip-v-mozg-ispytaniya-na-lyudyah-nachnutsya-cherez-polgoda-lrG1NQ")]
        [TestCase("https://www.goha.ru/naoki-joshida-nedovolen-povedeniem-igrokov-lWOBDY")]
        public async Task ParseGohaArticle_WithCorrectUrl_CorrectlyReturnsModel(string url)
        {
            var articleDto = await _htmlParserService.ParseGohaArticle(url);

            Assert.NotNull(articleDto);
        }

        [Test]
        [TestCase("https://auto.onliner.by/2023/02/24/v-minske-mogut-zapretit-parkovku-na-ulicax-po-opredelennym-dnyam-zimoj")]
        [TestCase("https://tech.onliner.by/2022/04/21/minyust-podreguliroval-izyatie-kriptovalyuty-u-dolzhnikov")]
        public async Task ParseGohaArticle_WithIncorrectUrl_ReturnsNullReferenceException(string url)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _htmlParserService.ParseGohaArticle(url));
        }
        #endregion

        #region ParseShazooArticle tests
        [Test]
        [TestCase("https://shazoo.ru/2022/04/27/125808/kosmiceskaya-strategiya-galactic-civilizations-iv-pokinula-rannii-dostup")]
        [TestCase("https://shazoo.ru/2022/04/04/124743/razrabotcik-abandoned-rasskazal-o-kontaktax-s-konami")]
        public async Task ParseShazooArticle_WithCorrectUrl_CorrectlyReturnsModel(string url)
        {
            var articleDto = await _htmlParserService.ParseShazooArticle(url);

            Assert.NotNull(articleDto);
        }

        [Test]
        [TestCase("https://auto.onliner.by/2023/02/24/v-minske-mogut-zapretit-parkovku-na-ulicax-po-opredelennym-dnyam-zimoj")]
        [TestCase("https://tech.onliner.by/2022/04/21/minyust-podreguliroval-izyatie-kriptovalyuty-u-dolzhnikov")]
        public async Task ParseShazooArticle_WithIncorrectUrl_ReturnsNullReferenceException(string url)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _htmlParserService.ParseShazooArticle(url));
        }
        #endregion
    }
}
