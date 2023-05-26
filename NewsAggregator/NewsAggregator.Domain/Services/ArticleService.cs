using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ArticleService(IMapper mapper,
            ILogger<ArticleService> logger,
            IUnitOfWork unitOfWork, 
            IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ArticleDto>> GetAllNewsAsync()
        {
            try
            {
                return await _unitOfWork.Articles.Get().Result
                    .Where(art => !string.IsNullOrWhiteSpace(art.Body))
                    .OrderByDescending(article => article.CreationDate)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetNewsByPageAsync(int page)
        {
            try
            {
                var pageSize = Convert.ToInt32(
                    _configuration[Variables.ConfigurationFields.PageSize]);
                return await _unitOfWork.Articles.Get().Result
                    .Where(art => !string.IsNullOrWhiteSpace(art.Body))
                    .OrderByDescending(article => article.CreationDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CreateOrEditArticleDto> GetArticleAsync(Guid Id)
        {
            try
            {
                var article = await _unitOfWork.Articles.GetById(Id);
                return _mapper.Map<CreateOrEditArticleDto>(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> CreateAsync(CreateOrEditArticleDto articleDto)
        {
            try
            {
                if (articleDto != null)
                {
                        await _unitOfWork.Articles.Add(_mapper.Map<Article>(articleDto));
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

        public async Task<int?> UpdateAsync(CreateOrEditArticleDto articleDto)
        {
            try
            {
                if (articleDto != null)
                {
                    await _unitOfWork.Articles.Update(_mapper.Map<Article>(articleDto));
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

        public async Task<int?> DeleteAsync(Guid modelId)
        {
            try
            {
                if (await _unitOfWork.Articles.GetById(modelId) != null)
                {
                    await _unitOfWork.Articles.Remove(modelId);
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




        public async Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id)
        {
            try
            {
                var model = await _unitOfWork.Articles.Get().Result
                    .Where(a => a.Id == id)
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                    .ThenInclude(user => user.User)
                    .FirstOrDefaultAsync();

                return _mapper.Map<ArticleDto>(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleWithAllNavigationPropertiesByRating(Guid id)
        {
            try
            {
                var model = await _unitOfWork.Articles.Get().Result
                    .Where(a => a.Id.Equals(id) && a.Coefficient > 0)
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                    .ThenInclude(user => user.User)
                    .FirstOrDefaultAsync();

                return _mapper.Map<ArticleDto>(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<List<string>> GetAllExistingArticleUrls()
        {
            try
            {
                return await _unitOfWork.Articles.Get().Result
                    .Select(article => article.SourceUrl)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }        
        }

        public async Task<ArticleDto> GetArticleWithoutRating()
        {
            try
            {
                var article = await _unitOfWork.Articles.Get().Result
                    .Where(article => article.Coefficient.Equals(null) && 
                        !string.IsNullOrWhiteSpace(article.Body))
                    .Take(1).FirstOrDefaultAsync();

                return _mapper.Map<ArticleDto>(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetAllNewsByRatingAsync()
        {
            try
            {
                return await _unitOfWork.Articles.Get().Result
                    .Where(article => article.Coefficient > 0)
                    .OrderByDescending(article => article.CreationDate)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetNewsByRatingByPageAsync(int page)
        {
            try
            {
                var pageSize = Convert.ToInt32(
                    _configuration[Variables.ConfigurationFields.PageSize]);

                return await _unitOfWork.Articles.Get().Result
                    .Where(article => article.Coefficient > 0)
                    .OrderByDescending(article => article.CreationDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<ArticleDto> MostRatedArticleByPeriodOfTime(float? maxCoef)
        {
            try
            {
                var article = await _unitOfWork.Articles.Get().Result
                    .Where(article => article.Coefficient.Equals(maxCoef))
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                        .ThenInclude(user => user.User)
                    .FirstOrDefaultAsync();

                return _mapper.Map<ArticleDto>(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<float?> MaxCoefOfToday()
        {
            try
            {
                var maxCoef = await _unitOfWork.Articles.Get().Result
                .Where(article => article.CreationDate.Date
                    .Equals(DateTime.Today.Date))
                    .MaxAsync(article => article.Coefficient);

                return maxCoef;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<float?> MaxCoefOfTheMonth()
        {
            try
            {
                var maxCoef = await _unitOfWork.Articles.Get().Result
                .Where(article => article.CreationDate.Date.Month
                    .Equals(DateTime.Today.Date.Month))
                    .MaxAsync(article => article.Coefficient);

                return maxCoef;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<float?> MaxCoefOfAllTime()
        {
            try
            {
                var maxCoef = await _unitOfWork.Articles.Get().Result
                    .MaxAsync(article => article.Coefficient);

                return maxCoef;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
