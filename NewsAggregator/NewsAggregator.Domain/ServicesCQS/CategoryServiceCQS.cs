using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class CategoryServiceCQS : ICategoryServiceCQS
    {
        private readonly ILogger<CategoryServiceCQS> _logger;
        private readonly IMediator _mediator;

        public CategoryServiceCQS(IMediator mediator, 
            ILogger<CategoryServiceCQS> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories()
        {
            try
            {
                return await _mediator.Send(new GetAllCategoriesQuery(),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryDto> GetCategoryAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetCategoryAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }

        public async Task<Guid> GetCategoryByUrlAsync(string url)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByUrlAsyncQuery(url), 
                    new CancellationToken());
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
                return await _mediator.Send(new GetCategoryByIdWithArticlesAsyncQuery(id),
                new CancellationToken());
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
                return await _mediator.Send(new GetCategoryByNameWithArticlesAsyncQuery(name),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }          
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesForAdminAsync(string name)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByNameWithArticlesForAdminAsyncQuery(name), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> CreateAsync(CategoryDto categoryDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> UpdateAsync(CategoryDto categoryDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
