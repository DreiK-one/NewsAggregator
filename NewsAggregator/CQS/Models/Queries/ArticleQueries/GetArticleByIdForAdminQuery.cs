using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleByIdForAdminQuery : IRequest<ArticleDto>
    {
        public GetArticleByIdForAdminQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
