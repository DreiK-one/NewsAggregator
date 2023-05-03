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


namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<UserService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IAccountService> _accountService;
        private UserService _userService;

        public UserServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<UserService>>();
            _accountService = new Mock<IAccountService>();

            _unitOfWork.Setup(uOw => uOw.Users.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestUserData.Users).Object);

            _userService = new UserService(_mapper,
                _logger.Object,
                _unitOfWork.Object,
                _accountService.Object);
        }
    }
}
