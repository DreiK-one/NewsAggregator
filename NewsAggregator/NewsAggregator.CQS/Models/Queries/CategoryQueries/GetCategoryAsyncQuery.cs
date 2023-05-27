using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryAsyncQuery : IRequest<CategoryDto>
    {
        public GetCategoryAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
