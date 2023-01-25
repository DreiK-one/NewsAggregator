using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregator.App.Mappers;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Domain.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

            _logger = new Mock<ILogger<CategoryService>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _categoryService = new CategoryService(_mapper, 
                _logger.Object, 
                _unitOfWork.Object);
        }

        [SetUp]
        public void Setup()
        {

        }
    }
}
