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
using System.Threading.Tasks;
using NewsAggregator.WebAPI.Mappers;
using NewsAggregator.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Data.Entities;
using NewsAggregator.Data;


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
                    mc.AddProfile(new UserProfile());
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
            _roleService = new Mock<IRoleService>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:Salt"]).Returns("qwerty123456");

            _unitOfWork.Setup(uOw => uOw.Users.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestUserData.Users).Object);

            _unitOfWork.Setup(uOw => uOw.UserRoles.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestUserRolesData.UserRoles).Object);

            _accountService = new AccountService(_mapper,
                _logger.Object,
                _unitOfWork.Object,
                _roleService.Object,
                _configuration.Object);
        }

        #region CheckUserWithThatEmailIsExistAsync tests
        [Test]
        [TestCase("andrew1993@gmail.com")]
        [TestCase("margo17@gmail.com")]
        public async Task CheckUserWithThatEmailIsExistAsync_WithExistEmail_ReturnsTrue(string email)
        {
            var ifExist = await _accountService
                .CheckUserWithThatEmailIsExistAsync(email);

            Assert.IsTrue(ifExist);
        }

        [Test]
        [TestCase("max14@gmail.com")]
        [TestCase("dany@gmail.ru")]
        public async Task CheckUserWithThatEmailIsExistAsync_WithNotExistEmail_ReturnsFalse(string email)
        {
            var ifExist = await _accountService.CheckUserWithThatEmailIsExistAsync(email);

            Assert.IsFalse(ifExist);
        }

        [Test]
        public async Task CheckUserWithThatEmailIsExistAsync_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _accountService.CheckUserWithThatEmailIsExistAsync(null));
        }
        #endregion

        #region GetUserIdByEmailAsync tests
        [Test]
        [TestCase("andrew1993@gmail.com")]
        [TestCase("margo17@gmail.com")]
        public async Task GetUserIdByEmailAsync_WithExistEmail_ReturnsCorrectId(string email)
        {
            var result = await _accountService.GetUserIdByEmailAsync(email);

            var expected = TestUserData.Users
                .FirstOrDefault(u => u.NormalizedEmail == email.ToUpperInvariant());

            Assert.AreEqual(expected.Id, result);
        }

        [Test]
        [TestCase("max14@gmail.com")]
        [TestCase("dany@gmail.ru")]
        public async Task GetUserIdByEmailAsync_WithNotExistEmail_ReturnsNull(string email)
        {
            var res = await _accountService.GetUserIdByEmailAsync(email);

            Assert.Null(res);
        }

        [Test]
        public async Task GetUserIdByEmailAsync_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _accountService.GetUserIdByEmailAsync(null));
        }
        #endregion

        #region GetUserById tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task GetUserById_WithCorrectId_ReturnsDto(Guid id)
        {
            var expected = new User { Id = id };

            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(expected);

            var res = await _accountService.GetUserById(id);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        [TestCase("b39c1ac0-4799-49b5-9bb6-074345173b6c")]
        [TestCase("ccd47653-dbe1-4495-a0e4-0d87f586ed29")]
        public async Task GetUserById_WithNotExistingId_ReturnsNull(Guid id)
        {
            var res = await _accountService.GetUserById(id);

            Assert.Null(res);
        }
        #endregion

        #region GetUserByEmailAsync tests
        [Test]
        [TestCase("andrew1993@gmail.com")]
        [TestCase("margo17@gmail.com")]
        public async Task GetUserByEmailAsync_WithCorrectEmail_ReturnsDto(string email)
        {
            var expected = TestUserData.Users
                .FirstOrDefault(u => u.NormalizedEmail == email.ToUpperInvariant());

            var res = await _accountService.GetUserByEmailAsync(email);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        [TestCase("andrew13@gmail.com")]
        [TestCase("margo67@gmail.ru")]
        public async Task GetUserByEmailAsync_WithNotExistingEmail_ReturnsNull(string email)
        {
            var res = await _accountService.GetUserByEmailAsync(email);

            Assert.Null(res);
        }

        [Test]
        public async Task GetUserByEmailAsync_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _accountService.GetUserByEmailAsync(null));
        }
        #endregion

        #region GetUserIdByNicknameAsync tests
        [Test]
        [TestCase("Andrew")]
        [TestCase("mick")]
        public async Task GetUserIdByNicknameAsync_WithCorrectNickname_ReturnsCorrectId(string nickname)
        {
            var expected = TestUserData.Users
                .FirstOrDefault(u => u.NormalizedNickname == nickname.ToUpperInvariant());

            var res = await _accountService
                .GetUserIdByNicknameAsync(nickname);

            Assert.AreEqual(res, expected.Id);
        }

        [Test]
        [TestCase("Nick")]
        [TestCase("Matt")]
        public async Task GetUserIdByNicknameAsync_WithNotExistingNickname_ReturnsEmptyGuid(string nickname)
        {
            var res = await _accountService
                .GetUserIdByNicknameAsync(nickname);

            Assert.AreEqual(res, Guid.Empty);
        }

        [Test]
        public async Task GetUserIdByNicknameAsync_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _accountService.GetUserIdByNicknameAsync(null));
        }
        #endregion

        #region GetUserNicknameByIdAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task GetUserNicknameByIdAsync_WithCorrectId_ReturnsNickname(Guid id)
        {
            var expected = TestUserData.Users.FirstOrDefault(u => u.Id == id);

            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(expected);

            var res = await _accountService
                .GetUserNicknameByIdAsync(id);

            Assert.AreEqual(res, expected.Nickname);
        }

        [Test]
        [TestCase("b39c1ac0-4799-49b5-9bb6-074345173b6c")]
        [TestCase("ccd47653-dbe1-4495-a0e4-0d87f586ed29")]
        public async Task GetUserNicknameByIdAsync_WithNotExistingId_ReturnsNull(Guid id)
        {
            var res = await _accountService.GetUserNicknameByIdAsync(id);

            Assert.Null(res);
        }
        #endregion

        #region CreateUserAsync tests
        [Test]
        [TestCase("dick", "dick@gmail.com")]
        [TestCase("hugh", "hugh@mail.ru")]
        public async Task CreateUserAsync_WithCorrectData_ReturnsNewId(string name, string email)
        {
            await _accountService.CreateUserAsync(name, email);

            _unitOfWork.Verify(uOw => uOw.Users.Add(It.IsAny<User>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task CreateUserAsync_WithNullData_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _accountService.CreateUserAsync(null, null));
        }
        #endregion

        #region SetRoleAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98", "User")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", "Admin")]
        public async Task SetRoleAsync_WithCorrectData_ReturnsSaveData(Guid userId, string roleName)
        {
            var roleId = Guid.NewGuid();
            _roleService.Setup(rs => rs.GetRoleIdByNameAsync(roleName))
                .ReturnsAsync(roleId);

            await _accountService.SetRoleAsync(userId, roleName);

            _unitOfWork.Verify(uOw => uOw.UserRoles.Add(It.IsAny<UserRole>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98", "User")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", "Admin")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", null)]
        public async Task SetRoleAsync_WithEmptyIdOrNullRoleName_ReturnsCreateRoleAndSaveData(Guid userId, string roleName)
        {
            var roleId = Guid.Empty;
            _roleService.Setup(rs => rs.GetRoleIdByNameAsync(roleName))
                .ReturnsAsync(roleId);

            await _accountService.SetRoleAsync(userId, roleName);

            _roleService.Verify(rs => rs.CreateRole(roleName));
            _unitOfWork.Verify(uOw => uOw.UserRoles.Add(It.IsAny<UserRole>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }
        #endregion

        #region GetRolesAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task GetRolesAsync_WithCorrectData_ReturnsSaveData(Guid userId)
        {
            _unitOfWork.Setup(uOw => uOw.Users.GetByIdWithIncludes(userId, u => u.UserRoles))
                .ReturnsAsync(TestUserData.Users.FirstOrDefault(u => u.Id == userId));

            var roles = await _accountService.GetRolesAsync(userId);

            Assert.NotNull(roles);

        }

        [Test]
        public async Task GetRolesAsync_WithEmpty_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _accountService.GetRolesAsync(Guid.Empty));
        }
        #endregion

        #region SetPasswordAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98", "asd123g")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", "xcv567dfg")]
        public async Task SetPasswordAsync_WithCorrectData_ReturnsSaveData(Guid userId, string password)
        {
            await _accountService.SetPasswordAsync(userId, password);

            _unitOfWork.Verify(uOw => uOw.Users.PatchAsync(userId, It.IsAny<List<PatchModel>>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task SetPasswordAsync_WithNull_ReturnsNullReferenceException(Guid userId)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await
                _accountService.SetPasswordAsync(userId, null));
        }
        #endregion

        #region CheckPasswordByEmailAsync tests
        [Test]
        [TestCase("andrew1993@gmail.com", "Pit_big123")]
        [TestCase("mick1990@mail.ru", "Mick_is_good!")]
        public async Task CheckPasswordByEmailAsync_WithCorrectData_ReturnsTrue(string email, string password)
        {
            var res = await _accountService.CheckPasswordByEmailAsync(email, password);

            Assert.True(res);
        }

        [Test]
        [TestCase("andrew1993@gmail.com", "Qwe_123")]
        [TestCase("mick1990@mail.ru", "Man_123-1")]
        public async Task CheckPasswordByEmailAsync_WithIncorrectData_ReturnsFalse(string email, string password)
        {
            var res = await _accountService.CheckPasswordByEmailAsync(email, password);

            Assert.False(res);
        }

        [Test]
        [TestCase("andrew1993@gmail.com", null)]
        [TestCase("mick1990@mail.ru", null)]
        public async Task CheckPasswordByEmailAsync_WithNullPassword_ReturnsFalse(string email, string password)
        {
            var res = await _accountService.CheckPasswordByEmailAsync(email, password);

            Assert.False(res);
        }

        [Test]
        public async Task CheckPasswordByEmailAsync_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _accountService.CheckPasswordByEmailAsync(null, null));
        }
        #endregion

        #region CheckPasswordByIdAsync tests
        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98", "Pit_big123")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", "Mick_is_good!")]
        public async Task CheckPasswordByIdAsync_WithCorrectData_ReturnsTrue(Guid id, string password)
        {
            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(TestUserData.Users.FirstOrDefault(u => u.Id == id));

            var res = await _accountService.CheckPasswordByIdAsync(id, password);

            Assert.True(res);
        }

        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98", "Jack_14123")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6", "Rick_13_1!")]
        public async Task CheckPasswordByIdAsync_WithIncorrectData_ReturnsFalse(Guid id, string password)
        {
            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(TestUserData.Users.FirstOrDefault(u => u.Id == id));

            var res = await _accountService.CheckPasswordByIdAsync(id, password);

            Assert.False(res);
        }

        [Test]
        [TestCase("f1dc4182-9459-49ea-a4d2-98f928c6da98")]
        [TestCase("08fe00bc-6f69-48bf-9d52-19a74a7be2f6")]
        public async Task CheckPasswordByIdAsync_WithNull_ReturnsFalse(Guid id)
        {
            _unitOfWork.Setup(uOw => uOw.Users.GetById(id))
                .ReturnsAsync(TestUserData.Users.FirstOrDefault(u => u.Id == id));

            var res = await _accountService.CheckPasswordByIdAsync(id, null);

            Assert.False(res);
        }
        #endregion

        #region UpdateEmail tests
        [Test]
        [TestCase("eb8e8568-2248-4ac0-95ac-14aeb2bc08cc", "Nick")]
        [TestCase("daf04b71-47ac-4615-aa78-92b3f85d3d93", "Glen")]
        public async Task UpdateEmail_WithCorrectData_CorrectSaveChanges(Guid id, string name)
        {
            await _accountService.UpdateEmail(id, name);

            _unitOfWork.Verify(uOw => uOw.Users.PatchAsync(id, It.IsAny<List<PatchModel>>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        [TestCase("eb8e8568-2248-4ac0-95ac-14aeb2bc08cc")]
        [TestCase("daf04b71-47ac-4615-aa78-92b3f85d3d93")]
        public async Task UpdateEmail_WithNull_ReturnsNullReferenceException(Guid id)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await
                _accountService.UpdateEmail(id, null));
        }
        #endregion

        #region UpdateNickname tests
        [Test]
        [TestCase("eb8e8568-2248-4ac0-95ac-14aeb2bc08cc", "Nick")]
        [TestCase("daf04b71-47ac-4615-aa78-92b3f85d3d93", "Glen")]
        public async Task UpdateNickname_WithCorrectData_CorrectSaveChanges(Guid id, string name)
        {
            await _accountService.UpdateNickname(id, name);

            _unitOfWork.Verify(uOw => uOw.Users.PatchAsync(id, It.IsAny<List<PatchModel>>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        [TestCase("eb8e8568-2248-4ac0-95ac-14aeb2bc08cc")]
        [TestCase("daf04b71-47ac-4615-aa78-92b3f85d3d93")]
        public async Task UpdateNickname_WithNull_ReturnsNullReferenceException(Guid id)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await 
                _accountService.UpdateNickname(id, null));
        }
        #endregion

        #region ValidateIsEmailExists tests
        [Test]
        [TestCase("andrew1993@gmail.com")]
        [TestCase("margo17@gmail.com")]
        public async Task ValidateIsEmailExists_WithExistEmail_ReturnsTrue(string email)
        {
            var ifExist = await Task.Run(() => 
                _accountService.ValidateIsEmailExists(email));

            Assert.IsTrue(ifExist);
        }

        [Test]
        [TestCase("max14@gmail.com")]
        [TestCase("dany@gmail.ru")]
        public async Task ValidateIsEmailExists_WithNotExistEmail_ReturnsFalse(string email)
        {
            var ifExist = await Task.Run(() => 
                _accountService.ValidateIsEmailExists(email));

            Assert.IsFalse(ifExist);
        }

        [Test]
        public async Task ValidateIsEmailExists_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await Task.Run(() => 
                _accountService.ValidateIsEmailExists(null)));
        }
        #endregion

        #region ValidateIsNicknameExists tests
        [Test]
        [TestCase("Andrew")]
        [TestCase("Mick")]
        public async Task ValidateIsNicknameExists_WithExistEmail_ReturnsTrue(string nickname)
        {
            var ifExist = await Task.Run(() =>
                _accountService.ValidateIsNicknameExists(nickname));

            Assert.IsTrue(ifExist);
        }

        [Test]
        [TestCase("Dave")]
        [TestCase("John")]
        public async Task ValidateIsNicknameExists_WithNotExistEmail_ReturnsFalse(string nickname)
        {
            var ifExist = await Task.Run(() =>
                _accountService.ValidateIsNicknameExists(nickname));

            Assert.IsFalse(ifExist);
        }

        [Test]
        public async Task ValidateIsNicknameExists_WithNull_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await Task.Run(() =>
                _accountService.ValidateIsNicknameExists(null)));
        }
        #endregion
    }
}
