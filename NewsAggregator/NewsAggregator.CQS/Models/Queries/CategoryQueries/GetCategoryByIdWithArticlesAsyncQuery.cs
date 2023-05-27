using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByIdWithArticlesAsyncQuery : IRequest<CategoryWithArticlesDto>
    {
        public GetCategoryByIdWithArticlesAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
