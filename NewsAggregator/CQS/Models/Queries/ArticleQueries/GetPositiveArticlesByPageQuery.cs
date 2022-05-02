using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetPositiveArticlesByPageQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public GetPositiveArticlesByPageQuery(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
