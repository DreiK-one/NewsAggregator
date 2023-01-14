using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregator.DataAccess;
using NewsAggregator.Domain.Services;
using NewsAggregator.Domain.Tests.Helpers;
using NUnit.Framework;


namespace NewsAggregator.Domain.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<ArticleService>> _logger;
        private Mock<IConfiguration> _configuration;
        private ArticleService _articleService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _logger= new Mock<ILogger<ArticleService>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(cfg => cfg["ApplicationVariables:PageSize"]).Returns("10");

            _articleService = new ArticleService(
                _mapper.Object, 
                _logger.Object, 
                _unitOfWork.Object, 
                _configuration.Object);
        }

        [Test]
        public async Task GetAllNewsAsync_CorrectlyReturnedListOfArticles()
        {
            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .Returns(TestFunctions.GetDbSet(TestArticlesData.Articles).Object);

            var articles = await _articleService.GetAllNewsAsync();

            Assert.IsNotNull(articles);
            Assert.AreEqual(5, articles.Count());
        }

        [Test]
        public async Task GetAllNewsByRatingAsync_ReturnedListOfArticlesWithPositiveRating()
        {
            _unitOfWork.Setup(uOw => uOw.Articles.Get())
                .Returns(TestFunctions.GetDbSet(TestArticlesData.Articles).Object);

            var articles = await _articleService.GetAllNewsByRatingAsync();

            Assert.IsNotNull(articles);
            Assert.AreEqual(3, articles.Count());
        }
    }
}
