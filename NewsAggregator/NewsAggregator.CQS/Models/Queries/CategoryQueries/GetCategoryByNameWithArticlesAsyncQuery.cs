using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByNameWithArticlesAsyncQuery : IRequest<CategoryWithArticlesDto>
    {
        public GetCategoryByNameWithArticlesAsyncQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
