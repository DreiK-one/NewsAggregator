using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticlesByPageQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public GetArticlesByPageQuery(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
