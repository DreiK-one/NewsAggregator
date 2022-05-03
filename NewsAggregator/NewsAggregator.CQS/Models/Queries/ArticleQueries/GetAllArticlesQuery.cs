using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
