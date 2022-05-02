using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllArticlesForAdminQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
