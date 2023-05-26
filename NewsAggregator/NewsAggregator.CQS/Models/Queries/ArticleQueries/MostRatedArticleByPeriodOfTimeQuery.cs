using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class MostRatedArticleByPeriodOfTimeQuery : IRequest<ArticleDto>
    {
        public MostRatedArticleByPeriodOfTimeQuery(float? maxCoef)
        {
            MaxCoef = maxCoef;
        }
        public float? MaxCoef { get; set; }
    }
}
