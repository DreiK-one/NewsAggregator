using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllNewsByRatingAsyncQuery : IRequest<IEnumerable<ArticleDto>>
    {
    }
}
