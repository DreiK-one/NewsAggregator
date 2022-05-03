using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllPositiveArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
