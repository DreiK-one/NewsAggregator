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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.Data.Entities;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.Domain.Tests.Services.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private IMapper _mapper;
        private Mock<ILogger<CategoryService>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private CategoryService _categoryService;

        public CategoryServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CategoryProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CategoryService>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            
            _unitOfWork.Setup(uOw => uOw.Categories.Get())
                .ReturnsAsync(TestFunctions.GetMockData(TestCategoriesData.Categories).Object);

            _categoryService = new CategoryService(_mapper,
                _logger.Object,
                _unitOfWork.Object);
        }

        #region GetAllCategoriesAsync tests
        [Test]
        public async Task GetAllCategoriesAsync_CorrectlyReturnedListOfCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            Assert.NotNull(categories);
            Assert.AreEqual(5, categories.Count());
        }

        [Test]
        public async Task GetAllCategoriesAsync_WithNoCategoriesInDb_ReturnedInvalidOperationException()
        {
            var nullList = new List<Category>().AsQueryable();
            await Task.Run(() => _unitOfWork.Setup(uOw => uOw.Categories.Get()).Returns(Task.FromResult(nullList)));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryService.GetAllCategoriesAsync());
        }
        #endregion

        #region CreateAsync tests
        [Test]
        public async Task CreateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var categoryDto = new CategoryDto
            {
                Id = Guid.NewGuid()
            };

            await _categoryService.CreateAsync(categoryDto);

            _unitOfWork.Verify(uOw => uOw.Categories.Add(It.IsAny<Category>()));
            _unitOfWork.Verify(uOw => uOw.Save());

        }

        [Test]
        public async Task CreateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _categoryService.CreateAsync(null));
        }
        #endregion

        #region UpdateAsync tests
        [Test]
        public async Task UpdateAsync_WithCorrectModel_ReturnsSaveResult()
        {
            var articleDto = new CategoryDto
            {
                Id = Guid.NewGuid()
            };

            await _categoryService.UpdateAsync(articleDto);

            _unitOfWork.Verify(uOw => uOw.Categories.Update(It.IsAny<Category>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task UpdateAsync_WithNullModel_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _categoryService.UpdateAsync(null));
        }
        #endregion

        #region DeleteAsync tests
        [Test]
        public async Task DeleteAsync_WithCorrectId_CorrectlyFinished()
        {
            var id = Guid.NewGuid();
            var expected = new Category { Id = id };

            _unitOfWork.Setup(uOw => uOw.Categories.GetById(id))
                .ReturnsAsync(expected);

            await _categoryService.DeleteAsync(id);

            _unitOfWork.Verify(uOw => uOw.Categories.Remove(It.IsAny<Guid>()));
            _unitOfWork.Verify(uOw => uOw.Save());
        }

        [Test]
        public async Task DeleteAsync_WithWrongId_ReturnsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _categoryService.DeleteAsync(It.IsAny<Guid>()));
        }
        #endregion
    }
}
