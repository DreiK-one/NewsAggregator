using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, 
            ILogger<CategoryService> logger, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                return await _unitOfWork.Categories.Get().Result
                .Select(category => _mapper.Map<CategoryDto>(category))
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> CreateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    var existCategory = (await _unitOfWork.Categories
                        .FindBy(c => c.Name.ToLower() == categoryDto.Name.ToLower()));

                    if (!existCategory.Any())
                    {
                        await _unitOfWork.Categories.Add(_mapper.Map<Category>(categoryDto));
                        return await _unitOfWork.Save();
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }                    
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> UpdateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    await _unitOfWork.Categories.Update(_mapper.Map<Category>(categoryDto));
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (await _unitOfWork.Categories.GetById(id) != null)
                {
                    await _unitOfWork.Categories.Remove(id);
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetCategoryByUrl(string url)
        {
            try
            {
                var str = url.Substring(8);
                var str2 = str.Remove(str.IndexOf('.'), str.Length - str.IndexOf('.'));
                var res = str2.Substring(0, 1).ToUpper() + (str2.Length > 1 ? str2.Substring(1) : "");

                var category = await _unitOfWork.Categories.Get().Result
                    .Select(category => category.Name).ToListAsync();

                if (category.Contains(res))
                {
                    return (await _unitOfWork.Categories.Get().Result
                    .FirstOrDefaultAsync(category => category.Name.Equals(res)))?.Id ?? Guid.Empty;
                }
                else
                {
                    return (await _unitOfWork.Categories.Get().Result
                    .FirstOrDefaultAsync(category => category.Name.Equals("Games")))?.Id ?? Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesAsync(string name)
        {
            try
            {
                var category = await _unitOfWork.Categories.Get().Result
                .Where(category => category.Name.Equals(name))
                .Include(article => article.Articles
                    .Where(article => article.Coefficient > 0)
                    .OrderByDescending(article => article.CreationDate))
                .FirstOrDefaultAsync();
                return _mapper.Map<CategoryWithArticlesDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw; ;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesForAdminAsync(string name)
        {
            try
            {
                var category = await _unitOfWork.Categories.Get().Result
                .Where(category => category.Name.Equals(name))
                .Include(article => article.Articles
                    .OrderByDescending(article => article.CreationDate))
                .FirstOrDefaultAsync();
                return _mapper.Map<CategoryWithArticlesDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw; ;
            }
        }

        public async Task<CategoryDto> GetCategoryAsync(Guid id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetById(id);
                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByIdWithArticlesAsync(Guid id)
        {
            try
            {
                var category = await _unitOfWork.Categories.Get().Result
                .Where(category => category.Id.Equals(id))
                .Include(article => article.Articles
                    .Where(article => article.Coefficient > 0)
                    .OrderByDescending(article => article.CreationDate))
                .FirstOrDefaultAsync();
                return _mapper.Map<CategoryWithArticlesDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw; ;
            }
        }
    }
}
