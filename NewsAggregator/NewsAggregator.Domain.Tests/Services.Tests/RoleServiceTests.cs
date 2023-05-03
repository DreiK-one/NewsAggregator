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

            _unitOfWork.Setup(uOw => uOw.UserRoles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestUserRolesData.UserRoles).Object);

            _roleService = new RoleService(_mapper,
                _logger.Object,
                _unitOfWork.Object);
        }

        #region GetAllRolesAsync tests
        [Test]
        public async Task GetAllRolesAsync_CorrectlyReturnedListOfRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();

            Assert.NotNull(roles);
            Assert.AreEqual(3, roles.ToList().Count());
        }

        [Test]
        public async Task GetAllRolesAsync_WithNoRolesInDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Role>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Roles.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.GetAllRolesAsync());
        }
        #endregion

        #region GetRoleIdByUserIdAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task GetRoleIdByUserIdAsync_WithCorrectId_ReturnsDto(Guid id)
        {
            var res = await _roleService.GetRoleIdByUserIdAsync(id);

            var expected = TestUserRolesData.UserRoles
                .Where(c => c.UserId == id).Select(r => r.RoleId).FirstOrDefault();

            Assert.AreEqual(res, expected);
        }

        [Test]
        public async Task GetUserByIdAsync_WithNull_ReturnsEmptyGuid()
        {
            var res = await _roleService.GetRoleIdByUserIdAsync(Guid.Empty);

            Assert.AreEqual(res, Guid.Empty);
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_WithCorrectModelAndNotExistedName_ReturnsSaveResult()
        {
            var roleDto = new RoleDto
            {
                Id = Guid.NewGuid(),
                Name = "Moderator"
            };

            await _roleService.CreateAsync(roleDto);

            _unitOfWork.Verify(uOw => uOw.Roles.Add(It.IsAny<Role>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task CreateAsync_WithCorrectModelAndExistedName_ReturnsNullReferenceException()
        {
            var roleDto = new RoleDto
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _roleService.CreateAsync(roleDto));
        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _roleService.CreateAsync(null));
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var roleDto = new RoleDto
            {
                Id = Guid.NewGuid(),
                Name = "Moderator"
            };

            await _roleService.UpdateAsync(roleDto);

            _unitOfWork.Verify(uOw => uOw.Roles.Update(It.IsAny<Role>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _roleService.UpdateAsync(null));
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new Role { Id = id };

            _unitOfWork.Setup(uOw => uOw.Roles.GetById(id))
                .ReturnsAsync(expected);

            await _roleService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Roles.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _roleService.DeleteAsync(It.IsAny<Guid>()));
        }
        #endregion

        #region GetRoleAsync tests
        [Test]
        [TestCase("bca65099-4302-49d7-b1a3-4d23d031739d")]
        [TestCase("f5f2fff5-62f7-486c-a1d6-314e5d99a36c")]
        public async Task GetRoleAsync_WithCorrectId_ReturnsDto(Guid id)
        {
            var expected = new Role { Id = id };

            _unitOfWork.Setup(uOw => uOw.Roles.GetById(id))
                .ReturnsAsync(expected);

            var res = await _roleService.GetRoleAsync(id);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        [TestCase("16da24dc-6d12-45d1-b2c3-3b62d39cb3ea")]
        [TestCase("4c407f85-21a2-4503-a2d4-ba2a0c1c8586")]
        public async Task GetRoleAsync_WithNotExistingId_ReturnsNull(Guid id)
        {
            var res = await _roleService.GetRoleAsync(id);

            Assert.Null(res);
        }
        #endregion 
    }
}
