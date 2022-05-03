using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryWithArticlesDto>>
    {
    }
}
