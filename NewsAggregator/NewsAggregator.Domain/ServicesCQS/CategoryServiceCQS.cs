using CQS.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class CategoryServiceCQS : ICategoryServiceCQS
    {
        private readonly IMediator _mediator;

        public CategoryServiceCQS(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CategoryWithArticlesDto> GetCategoryById(Guid id)
        {
            return await _mediator.Send(new GetCategoryByIdQuery(id), 
                new CancellationToken());
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByName(string name)
        {
            return await _mediator.Send(new GetCategoryByNameQuery(name), 
                new CancellationToken());
        }

        public async Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories()
        {
            return await _mediator.Send(new GetAllCategoriesQuery(), new CancellationToken());
        }
    }
}
