using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return await _unitOfWork.Articles.Get()
                    .OrderByDescending(article => article.CreationDate)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetNewsByPageAsync(int page)
        {
            try
            {
                var pageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);
                return await _unitOfWork.Articles.Get()
                    .OrderByDescending(article => article.CreationDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id)
        {
            try
            {
                var model = await _unitOfWork.Articles.Get()
                    .Where(a => a.Id.Equals(id))
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                    .ThenInclude(user => user.User)
                    .FirstOrDefaultAsync();

                return _mapper.Map<ArticleDto>(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<string>> GetAllExistingArticleUrls()
        {
            try
            {
                return await _unitOfWork.Articles.Get()
                    .Select(article => article.SourceUrl)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }        
        }
    }
}
