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
using System.Threading.Tasks;
using System.Linq;
using NewsAggregator.Data.Entities;
using System.Collections.Generic;
using System;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class RoleServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<RoleService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private RoleService _roleService;

        public RoleServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new RoleProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<RoleService>>();

            _unitOfWork.Setup(uOw => uOw.Roles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestRoleData.Roles).Object);

            _roleService = new RoleService(_mapper,
                _logger.Object,
                _unitOfWork.Object);
        }
    }
}
