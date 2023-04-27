using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces;
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
    }
}
