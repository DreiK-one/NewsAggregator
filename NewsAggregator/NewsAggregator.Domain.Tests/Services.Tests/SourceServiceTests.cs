using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;


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
    }
}
