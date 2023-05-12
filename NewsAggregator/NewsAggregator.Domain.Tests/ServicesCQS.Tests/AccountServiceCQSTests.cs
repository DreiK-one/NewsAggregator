using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class AccountServiceCQSTests
    {
        private AccountServiceCQS _accountServiceCQS;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<AccountServiceCQS>> _logger;
        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<AccountServiceCQS>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _accountServiceCQS = new AccountServiceCQS(
                _logger.Object,
                _configuration.Object,
                _mediator.Object);
        }

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