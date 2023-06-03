using MediatR;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleWithEmptyBodyAndTitleQuery : IRequest<Article>
    {
    }
}
