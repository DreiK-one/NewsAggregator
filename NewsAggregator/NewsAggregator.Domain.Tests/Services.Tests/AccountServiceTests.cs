using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.WebAPI.Mappers;
using NewsAggregator.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<AccountService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IRoleService> _roleService;
        private Mock<IConfiguration> _configuration;
        private AccountService _accountService;

        public AccountServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AccountProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AccountService>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _configuration = new Mock<IConfiguration>();

            //_configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _unitOfWork.Setup(uOw => uOw.Categories.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestCategoriesData.Categories).Object);

            _accountService = new AccountService(_mapper,
                _logger.Object,
                _unitOfWork.Object,
                _roleService.Object,
                _configuration.Object);
        }
    }
}
