using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.ServicesCQS;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests.ServicesCQS.Tests
{
    [TestFixture]
    public class CategoryServiceCQSTests
    {
        private Mock<ILogger<CategoryServiceCQS>> _logger;
        private Mock<IMediator> _mediator;
        private CategoryServiceCQS _categoryServiceCQS;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CategoryServiceCQS>>();

            _categoryServiceCQS = new CategoryServiceCQS(
                _mediator.Object,
                _logger.Object);
        }

        #region GetCategoryById tests
        [Test]
        [TestCase("F50BF279-C389-4B8E-AAC1-117FFBCAE899")]
        [TestCase("83B34299-C56B-402E-95E9-43E0367D1239")]
        public async Task GetCategoryById_ExistingId_CorrectlyReturnedDto(Guid id)
        {
            var dto = new CategoryWithArticlesDto()
            {
                Id = id
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var category = await _categoryServiceCQS.GetCategoryById(id);

            Assert.AreEqual(dto.Id, category.Id);
        }

        [Test]
        [TestCase("C2340D56-DBAA-4039-B0A1-0016A22C4322")]
        public async Task GetCategoryById_NoExistentId_ReturnedNull(Guid id)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var category = await _categoryServiceCQS.GetCategoryById(id);

            Assert.Null(category);
        }
        #endregion

        #region GetCategoryByName tests
        [Test]
        [TestCase("Sport")]
        [TestCase("People")]
        public async Task GetCategoryByName_ExistingName_CorrectlyReturnedDto(string name)
        {
            var dto = new CategoryWithArticlesDto()
            {
                Name = name
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByNameQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => dto);

            var category = await _categoryServiceCQS.GetCategoryByName(name);

            Assert.AreEqual(dto.Name, category.Name);
        }

        [Test]
        public async Task GetCategoryByName_NoExistentName_ReturnedNull()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var category = await _categoryServiceCQS.GetCategoryByName("");

            Assert.Null(category);
        }
        #endregion

        #region GetAllCategories tests
        [Test]
        public async Task GetAllCategories_CorrectlyReturnsCategories()
        {
            var expected = new List<CategoryWithArticlesDto>()
            {
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
                new CategoryWithArticlesDto(),
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expected);

            var categories = await _categoryServiceCQS.GetAllCategories();

            Assert.AreEqual(categories.ToList().Count, expected.Count());
        }
        #endregion
    }
}