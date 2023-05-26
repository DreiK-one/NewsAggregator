using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleWithAllNavigationPropertiesByRatingQuery : IRequest<ArticleDto>
    {
        public GetArticleWithAllNavigationPropertiesByRatingQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
