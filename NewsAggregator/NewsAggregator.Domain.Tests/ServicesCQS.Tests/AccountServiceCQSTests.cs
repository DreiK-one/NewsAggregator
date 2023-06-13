using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using NewsAggregetor.CQS.Models.Queries.UserRoleQueries;
using NUnit.Framework;
using System.Security.Cryptography;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class AccountServiceCQSTests
    {
        private AccountServiceCQS _accountServiceCQS;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<AccountServiceCQS>> _logger;
        private Mock<IConfiguration> _configuration;
        private Mock<IRoleServiceCQS> _roleServiceCQS;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<AccountServiceCQS>>();
            _configuration = new Mock<IConfiguration>();
            _roleServiceCQS = new Mock<IRoleServiceCQS>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");
            _configuration.Setup(cfg => cfg["ApplicationVariables:Salt"]).Returns("asd1234ad");

            _accountServiceCQS = new AccountServiceCQS(
                _logger.Object,
                _configuration.Object,
                _mediator.Object,
                _roleServiceCQS.Object);
        }

        #region GetUserByEmailAsync tests
        [Test]
        [TestCase("test1@gmail.com")]
        [TestCase("test2@gmail.com")]
        public async Task GetUserByEmailAsync_ExistingEmail_ReturnedDto(string email)
        {
            var dto = new UserDto()
            {
                Email = email
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var user = await _accountServiceCQS.GetUserByEmailAsync(email);

            Assert.AreEqual(dto.Email, user.Email);
        }

        [Test]
        [TestCase("nonexist@mail.com")]
        public async Task GetUserByEmailAsync_NoExistentEmail_ReturnedNull(string email)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var user = await _accountServiceCQS.GetUserByEmailAsync(email);

            Assert.Null(user);
        }
        #endregion

        #region GetUserByIdAsync tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetArticleById_ExistingId_ReturnedDto(Guid id)
        {
            var dto = new UserDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var user = await _accountServiceCQS.GetUserByIdAsync(id);

            Assert.AreEqual(dto.Id, user.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312")]
        public async Task GetUserByIdAsync_NoExistentId_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var user = await _accountServiceCQS.GetUserByIdAsync(id);

            Assert.Null(user);
        }
        #endregion

        #region GetUserByRefreshTokenAsync tests
        [Test]
        [TestCase("test1@gmail.com")]
        [TestCase("test2@gmail.com")]
        public async Task GetUserByRefreshTokenAsync_ExistingToken_ReturnedDto(string token)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserByRefreshToken>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new UserDto());

            var user = await _accountServiceCQS.GetUserByRefreshTokenAsync(token);

            Assert.NotNull(user);
        }

        [Test]
        [TestCase("nonexist@mail.com")]
        public async Task GetUserByRefreshTokenAsync_NoExistentToken_ReturnedNull(string token)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserByRefreshToken>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var user = await _accountServiceCQS.GetUserByRefreshTokenAsync(token);

            Assert.Null(user);
        }
        #endregion

        #region GetUserIdByEmailAsync tests
        [Test]
        [TestCase("test@gmail.com")]
        [TestCase("test2@mail.ru")]
        public async Task GetUserIdByEmailAsync_ExistingEmail_ReturnedGuid(string email)
        {
            var expected = Guid.NewGuid();

            _mediator.Setup(m => m.Send(It.IsAny<GetUserIdByEmailAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var userId = await _accountServiceCQS.GetUserIdByEmailAsync(email);

            Assert.AreEqual(expected, userId);
        }

        [Test]
        [TestCase("test3@mail.ru")]
        public async Task GetUserIdByEmailAsync_NoExistentEmail_ReturnedNull(string email)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserIdByEmailAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var userId = await _accountServiceCQS.GetUserIdByEmailAsync(email);

            Assert.Null(userId);
        }
        #endregion

        #region GetUserIdByNicknameAsync tests
        [Test]
        [TestCase("TestNickname")]
        [TestCase("TestNickname2")]
        public async Task GetUserIdByNicknameAsync_ExistingNickname_ReturnedGuid(string nickname)
        {
            var expected = Guid.NewGuid();

            _mediator.Setup(m => m.Send(It.IsAny<GetUserIdByNicknameAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var userId = await _accountServiceCQS.GetUserIdByNicknameAsync(nickname);

            Assert.AreEqual(expected, userId);
        }

        [Test]
        [TestCase("NotExNick")]
        public async Task GetUserIdByNicknameAsync_NoExistentNickname_ReturnedNull(string nickname)
        {
            var expected = Guid.Empty;

            _mediator.Setup(m => m.Send(It.IsAny<GetUserIdByNicknameAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var userId = await _accountServiceCQS.GetUserIdByNicknameAsync(nickname);

            Assert.AreEqual(expected, userId);
        }
        #endregion

        #region GetUserNicknameByIdAsync tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetUserNicknameByIdAsync_ExistingId_ReturnedNickname(Guid id)
        {
            var expected = "Existed nickname";

            _mediator.Setup(m => m.Send(It.IsAny<GetUserNicknameByIdAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var userNickname = await _accountServiceCQS.GetUserNicknameByIdAsync(id);

            Assert.AreEqual(expected, userNickname);
        }

        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetUserNicknameByIdAsync_NoExistentId_ReturnedNull(Guid id)
        {
            string? expected = null;

            _mediator.Setup(m => m.Send(It.IsAny<GetUserNicknameByIdAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var userId = await _accountServiceCQS.GetUserNicknameByIdAsync(id);

            Assert.AreEqual(expected, userId);
        }
        #endregion

        #region GetRolesAsync tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350")]
        public async Task GetRolesAsync_ExistingId_ReturnedListOfNicknames(Guid id)
        {
            var expected = new List<Guid> 
            { 
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }.AsEnumerable();

            _mediator.Setup(m => m.Send(It.IsAny<GetUserRoleIdsByUserIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            _roleServiceCQS.Setup(rs => rs.GetRoleNameByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync("someName");

            var names = await _accountServiceCQS.GetRolesAsync(id);

            Assert.NotNull(names);
            Assert.AreEqual(expected.Count(), names.Count());
        }

        [Test]
        public async Task GetRolesAsync_NoExistentId_ReturnedNullList()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetUserNicknameByIdAsyncQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var names = await _accountServiceCQS.GetRolesAsync(Guid.NewGuid());

            Assert.AreEqual(0, names.Count());
        }
        #endregion



        #region SetPasswordAsync tests
        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6", "asd1234")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4350", "fgh567dg")]
        public async Task SetPasswordAsync_WithFilledPassword_CorrectlyReturnedResult(Guid id, string password)
        {
            _mediator.Setup(m => m.Send(It.IsAny<SetPasswordCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<int>());

            var res = await _accountServiceCQS.SetPasswordAsync(id, password);

            Assert.AreEqual(It.IsAny<int>(), res);
        }

        [Test]
        public async Task SetPasswordAsync_NoExistentId_ReturnedNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _accountServiceCQS.SetPasswordAsync(Guid.NewGuid(), ""));
        }
        #endregion

        #region CheckPasswordByEmailAsync tests
        [Test]
        [TestCase("asd12345aqweq")]
        [TestCase("asdasdfadsdfa123")]
        public async Task CheckPasswordByEmailAsync_WithCorrectPasswordAndEmail_ReturnedTrue(string password)
        {
            var dto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                PasswordHash = ""
            };

            var sha1 = new SHA1CryptoServiceProvider();
            var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{"asd1234ad"}_{password}"));
            var hashedPassword = Encoding.UTF8.GetString(sha1Data);
            dto.PasswordHash = hashedPassword;

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var res = await _accountServiceCQS
                .CheckPasswordByEmailAsync("test@mail.com", password);

            Assert.AreEqual(true, res);
        }

        [Test]
        [TestCase("asd12345aqweq")]
        [TestCase("")]
        public async Task CheckPasswordByEmailAsync_WithWrongOrEmptyPassword_ReturnedFalse(string password)
        {
            var dto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                PasswordHash = password
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var res = await _accountServiceCQS
                .CheckPasswordByEmailAsync("test@mail.com", password);

            Assert.AreEqual(false, res);
        }

        [Test]
        [TestCase("wrong1@mail.com", "asd12345aqweq")]
        [TestCase("wrong2@mail.com", "adsayui124g")]
        public async Task CheckPasswordByEmailAsync_WithWrongEmail_ReturnedNullReferenceException(string email, string password)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _accountServiceCQS.CheckPasswordByEmailAsync(email, password));
        }
        #endregion

        #region CheckPasswordByIdAsync tests
        [Test]
        [TestCase("asd12345aqweq")]
        [TestCase("asdasdfadsdfa123")]
        public async Task CheckPasswordByIdAsync_WithCorrectPasswordAndId_ReturnedTrue(string password)
        {
            var dto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                PasswordHash = ""
            };

            var sha1 = new SHA1CryptoServiceProvider();
            var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{"asd1234ad"}_{password}"));
            var hashedPassword = Encoding.UTF8.GetString(sha1Data);
            dto.PasswordHash = hashedPassword;

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var res = await _accountServiceCQS
                .CheckPasswordByIdAsync(Guid.NewGuid(), password);

            Assert.AreEqual(true, res);
        }

        [Test]
        [TestCase("asd12345aqweq")]
        [TestCase("")]
        public async Task CheckPasswordByIdAsync_WithWrongOrEmptyPassword_ReturnedFalse(string password)
        {
            var dto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                PasswordHash = password
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var res = await _accountServiceCQS
                .CheckPasswordByIdAsync(Guid.NewGuid(), password);

            Assert.AreEqual(false, res);
        }

        [Test]
        [TestCase("AF53DA74-A935-47E0-B372-000499DDEAA6", "asd12345aqweq")]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4312", "adsayui124g")]
        public async Task CheckPasswordByIdAsync_WithWrongId_ReturnedNullReferenceException(Guid id, string password)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _accountServiceCQS.CheckPasswordByIdAsync(id, password));
        }
        #endregion




        #region ValidateIsNicknameExists tests
        [Test]
        [TestCase("Pit1992")]
        [TestCase("greg17")]
        public async Task ValidateIsNicknameExists_ExistingNickname_ReturnedTrue(string nickname)
        {
            _mediator.Setup(m => m.Send(It.IsAny<ValidateNicknameQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => true);

            var isExist = await _accountServiceCQS.ValidateIsNicknameExists(nickname);

            Assert.AreEqual(isExist, true);
        }

        [Test]
        [TestCase("megg18")]
        public async Task ValidateIsNicknameExists_NoExistentNickname_ReturnedFalse(string nickname)
        {
            _mediator.Setup(m => m.Send(It.IsAny<ValidateNicknameQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => false);

            var isExist = await _accountServiceCQS.ValidateIsNicknameExists(nickname);

            Assert.AreEqual(isExist, false);
        }
        #endregion

        #region ValidateIsEmailExists tests
        [Test]
        [TestCase("test1@gmail.com")]
        [TestCase("test2@gmail.com")]
        public async Task ValidateIsEmailExists_ExistingEmailReturnedTrue(string email)
        {
            _mediator.Setup(m => m.Send(It.IsAny<ValidateEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => true);

            var isExist = await _accountServiceCQS.ValidateIsEmailExists(email);

            Assert.AreEqual(isExist, true);
        }

        [Test]
        [TestCase("nonexist@mail.com")]
        public async Task ValidateIsEmailExists_NoExistentEmail_ReturnedFalse(string email)
        {
            _mediator.Setup(m => m.Send(It.IsAny<ValidateEmailQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => false);

            var isExist = await _accountServiceCQS.ValidateIsEmailExists(email);

            Assert.AreEqual(isExist, false);
        }
        #endregion
    }
}