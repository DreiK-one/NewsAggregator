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
                .ReturnsAsync(TestFunctions.GetMockData(TestUserData.UsersForUserService).Object);

            _unitOfWork.Setup(uOw => uOw.UserRoles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestUserRolesData.UserRoles).Object);

            _userService = new UserService(_mapper,
                _logger.Object,
                _unitOfWork.Object,
                _accountService.Object);
        }

        #region GetAllUsersWithAllInfoAsync tests
        [Test]
        public async Task GetAllUsersWithAllInfoAsync_CorrectlyReturnedListOfUsers()
        {
            var users = await _userService.GetAllUsersWithAllInfoAsync();

            Assert.NotNull(users);
            Assert.AreEqual(3, users.ToList().Count());
        }

        [Test]
        public async Task GetAllUsersWithAllInfoAsync_WithNoUsersInDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<User>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Users.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetAllUsersWithAllInfoAsync());
        }
        #endregion

        #region GetUserByIdAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task GetUserByIdAsync_WithCorrectId_ReturnsDto(Guid id)
        {

            var res = await _userService
                .GetUserByIdAsync(id);

            var expected = TestUserData.UsersForUserService
                .FirstOrDefault(c => c.Id == res.Id);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        public async Task GetUserByIdAsync_WithNull_ReturnsNull()
        {
            var res = await _userService.GetUserByIdAsync(Guid.Empty);

            Assert.Null(res);
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var userDto = new CreateOrEditUserDto
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Nickname = "testNickname"
            };

            await _userService.UpdateAsync(userDto);

            _unitOfWork.Verify(uOw => uOw.Users.PatchAsync(userDto.Id, It.IsAny<List<PatchModel>>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _userService.UpdateAsync(null));
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new User { Id = id };

            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(expected);

            await _userService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Users.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _userService.DeleteAsync(It.IsAny<Guid>()));
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var userDto = new CreateOrEditUserDto
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Nickname = "testNickname",
                RoleId = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                PasswordHash = ""
            };

            await _userService.CreateAsync(userDto);

            _unitOfWork.Verify(uOw => uOw.UserRoles.Add(It.IsAny<UserRole>()));
            _unitOfWork.Verify(uOw => uOw.Users.Add(It.IsAny<User>()));
            _unitOfWork.Verify(uOw => uOw.Save());

        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _userService.CreateAsync(null));
        }
        #endregion
    }
}
