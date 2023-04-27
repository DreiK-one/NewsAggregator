using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers.TestData;
using NewsAggregator.Domain.Tests.Services.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using NewsAggregator.Data.Entities;
using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class CommentServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<CommentService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private CommentService _commentService;

        public CommentServiceTests()
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
            _logger = new Mock<ILogger<CommentService>>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.Setup(uOw => uOw.Comments.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestCommentsData.Comments).Object);

            _commentService = new CommentService(_mapper,
                _logger.Object,
                _unitOfWork.Object);
        }

        #region GetCommentAsync tests
        [Test]
        [TestCase("1d047401-95fb-4e18-a946-72c6a3062a7a")]
        [TestCase("fbfa3257-71a5-445a-b58a-e9483aaec298")]
        public async Task GetCommentAsync_WithCorrectId_ReturnsDto(Guid id)
        {
            var expected = new Comment { Id = id };

            _unitOfWork.Setup(uOw => uOw.Comments.GetById(id))
                .ReturnsAsync(expected);

            var res = await _commentService.GetCommentAsync(id);

            Assert.AreEqual(res.Id, expected.Id);
        }

        [Test]
        public async Task GetCommentAsync_WithNull_ReturnsNull()
        {
            var res = await _commentService.GetCommentAsync(Guid.Empty);

            Assert.Null(res);
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var commentDto = new CreateOrEditCommentDto
            {
                Id = Guid.NewGuid()
            };

            await _commentService.CreateAsync(commentDto);

            _unitOfWork.Verify(uOw => uOw.Comments.Add(It.IsAny<Comment>()));
            _unitOfWork.Verify(uOw => uOw.Save());

        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _commentService.CreateAsync(null));
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var commentDto = new CreateOrEditCommentDto
            {
                Id = Guid.NewGuid()
            };

            await _commentService.UpdateAsync(commentDto);

            _unitOfWork.Verify(uOw => uOw.Comments.Update(It.IsAny<Comment>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _commentService.UpdateAsync(null));
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new Comment { Id = id };

            _unitOfWork.Setup(uOw => uOw.Comments.GetById(id))
                .ReturnsAsync(expected);

            await _commentService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Comments.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _commentService.DeleteAsync(It.IsAny<Guid>()));
        }
        #endregion
    }
}
