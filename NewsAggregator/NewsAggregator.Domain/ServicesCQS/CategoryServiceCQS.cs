using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
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

        public async Task<CategoryWithArticlesDto> GetCategoryById(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByIdQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByName(string name)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByNameQuery(name),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }          
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
