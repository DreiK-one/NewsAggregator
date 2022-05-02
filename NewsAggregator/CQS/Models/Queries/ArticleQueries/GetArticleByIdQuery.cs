using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleByIdQuery : IRequest<ArticleDto>
    {
        public GetArticleByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
