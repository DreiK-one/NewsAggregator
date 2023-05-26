using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetNewsByRatingByPageAsyncQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public GetNewsByRatingByPageAsyncQuery(int page)
        {
            Page = page;
        }
        public int Page{ get; set; }
    }
}
