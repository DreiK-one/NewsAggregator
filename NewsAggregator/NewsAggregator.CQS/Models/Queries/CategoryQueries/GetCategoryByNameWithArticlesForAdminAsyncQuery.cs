using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByNameWithArticlesForAdminAsyncQuery : IRequest<CategoryWithArticlesDto>
    {
        public GetCategoryByNameWithArticlesForAdminAsyncQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
