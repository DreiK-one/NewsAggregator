using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllArticlesForUserQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
