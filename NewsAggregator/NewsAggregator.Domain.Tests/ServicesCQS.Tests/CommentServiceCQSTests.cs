using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregator.WebAPI.Mappers;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using NewsAggregetor.CQS.Models.Queries.CommentQueries;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class CommentServiceCQSTests
    {   
        private IMapper _mapper;
        private Mock<ILogger<CommentServiceCQS>> _logger;
        private Mock<IMediator> _mediator;
        private CommentServiceCQS _commentServiceCQS;

        public CommentServiceCQSTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CommentProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CommentServiceCQS>>();

            _commentServiceCQS = new CommentServiceCQS(
                _mapper,
                _logger.Object,
                _mediator.Object);
        }

        #region GetByIdAsync tests
        [Test]
        [TestCase("F50BF279-C389-4B8E-AAC1-117FFBCAE899")]
        [TestCase("83B34299-C56B-402E-95E9-43E0367D1239")]
        public async Task GetByIdAsync_ExistingId_CorrectlyReturnedDto(Guid id)
        {
            var dto = new CreateOrEditCommentDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCommentByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var comment = await _commentServiceCQS.GetByIdAsync(id);

            Assert.AreEqual(dto.Id, comment.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4322")]
        public async Task GetByIdAsync_NoExistentId_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetCommentByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var comment = await _commentServiceCQS.GetByIdAsync(id);

            Assert.Null(comment);
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task GetByIdAsync_CorrectlyReturnedResult()
        {
            var dto = new CreateOrEditCommentDto()
            {
                Id = Guid.NewGuid()
            };

            _mediator.Setup(m => m.Send(It.IsAny<CreateCommentCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<bool>());

            var comment = await _commentServiceCQS.CreateAsync(dto);

            Assert.AreEqual(comment, false);
        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnedFalse()
        {
            var comment = await _commentServiceCQS.CreateAsync(null);

            Assert.AreEqual(comment, false);
        }
        #endregion

        #region EditAsync tests
        [Test]
        public async Task EditAsync_CorrectlyReturnedResult()
        {
            var dto = new CreateOrEditCommentDto()
            {
                Id = Guid.NewGuid()
            };

            _mediator.Setup(m => m.Send(It.IsAny<EditCommentCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<bool>());

            var comment = await _commentServiceCQS.EditAsync(dto);

            Assert.AreEqual(comment, false);
        }

        [Test]
        public async Task EditAsync_WithNullModel_ReturnedFalse()
        {
            var comment = await _commentServiceCQS.EditAsync(null);

            Assert.AreEqual(comment, false);
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_CorrectlyReturnedResult()
        {
            _mediator.Setup(m => m.Send(It.IsAny<DeleteCommentCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => It.IsAny<bool>());

            var comment = await _commentServiceCQS.DeleteAsync(Guid.NewGuid());

            Assert.AreEqual(comment, false);
        }
        #endregion
    }
}