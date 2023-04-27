using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using NewsAggregator.Data.Entities;
using System.Collections.Generic;
using System;
using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class SourceServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<SourceService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private SourceService _sourceService;

        public SourceServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new SourceProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<SourceService>>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.Setup(uOw => uOw.Sources.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestSourcesData.Sources).Object);

            _sourceService = new SourceService(_mapper,
                _logger.Object,
                _unitOfWork.Object);
        }

        #region GetAllSourcesAsync tests
        [Test]
        public async Task GetAllSourcesAsync_CorrectlyReturnedListOfSources()
        {
            var sources = await _sourceService.GetAllSourcesAsync();

            Assert.NotNull(sources);
            Assert.AreEqual(3, sources.ToList().Count());
        }

        [Test]
        public async Task GetAllSourcesAsync_WithNoSourcesInDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Source>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Sources.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _sourceService.GetAllSourcesAsync());
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var sourceDto = new SourceDto
            {
                Id = Guid.NewGuid()
            };

            await _sourceService.CreateAsync(sourceDto);

            _unitOfWork.Verify(uOw => uOw.Sources.Add(It.IsAny<Source>()));
            _unitOfWork.Verify(uOw => uOw.Save());

        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sourceService.CreateAsync(null));
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var sourceDto = new SourceDto
            {
                Id = Guid.NewGuid()
            };

            await _sourceService.UpdateAsync(sourceDto);

            _unitOfWork.Verify(uOw => uOw.Sources.Update(It.IsAny<Source>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sourceService.UpdateAsync(null));
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new Source { Id = id };

            _unitOfWork.Setup(uOw => uOw.Sources.GetById(id))
                .ReturnsAsync(expected);

            await _sourceService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Sources.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sourceService.DeleteAsync(It.IsAny<Guid>()));
        }
        #endregion

        #region GetRssUrlsAsync tests
        [Test]
        public async Task GetRssUrlsAsync_CorrectlyReturnedListOfRssUrls()
        {
            var sources = await _sourceService.GetRssUrlsAsync();

            Assert.NotNull(sources);
            Assert.AreEqual(3, sources.ToList().Count());
        }

        [Test]
        public async Task GetRssUrlsAsync_WithNoRssUrlsInDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Source>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Sources.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _sourceService.GetRssUrlsAsync());
        }
        #endregion

        #region GetSourceByUrl tests
        [Test]
        [TestCase("http://onliner.by")]
        [TestCase("http://goha.ru")]
        [TestCase("http://shazoo.ru")]
        public async Task GetSourceByUrl_WithCorrectUrl_ReturnsGuid(string url)
        {
            var res = await _sourceService.GetSourceByUrl(url);

            var expected = TestSourcesData.Sources.FirstOrDefault(c => c.Id == res);

            Assert.AreEqual(res, expected.Id);
        }

        [Test]
        public async Task GetSourceByUrl_WithNull_ReturnsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sourceService.GetSourceByUrl(null));
        }

        [Test]
        [TestCase("www.onliner.by")]
        [TestCase("goha.ru")]
        [TestCase("some.string")]
        public async Task GetSourceByUrl_WithNotUri_ReturnsUriFormatException(string wrongUrl)
        {
            Assert.ThrowsAsync<UriFormatException>(async () => await _sourceService.GetSourceByUrl(wrongUrl));
        }
        #endregion

        #region GetSourceAsync tests
        [Test]
        [TestCase("cdb6ae1b-36f0-4dae-832c-1315e1e0634a")]
        [TestCase("31ecd07b-8e20-42c0-8b12-8d3f0b7b2be2")]
        public async Task GetSourceAsync_WithCorrectId_ReturnsDto(Guid id)
        {
            var expected = new Source { Id = id };

            _unitOfWork.Setup(uOw => uOw.Sources.GetById(id))
                .ReturnsAsync(expected);

            var res = await _sourceService.GetSourceAsync(id);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        [TestCase("051a4774-009b-44e8-96ca-67e2184ef6f8")]
        [TestCase("18959597-0808-4ea6-a442-eeee9b66937b")]
        public async Task GetSourceAsync_WithNotExistingId_ReturnsNull(Guid id)
        {
            var res = await _sourceService.GetSourceAsync(id);

            Assert.Null(res);
        }
        #endregion
    }
}
