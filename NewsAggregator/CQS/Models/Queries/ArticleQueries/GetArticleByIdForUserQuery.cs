using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleByIdForUserQuery : IRequest<ArticleDto>
    {
        public GetArticleByIdForUserQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
