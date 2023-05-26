using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class MaxCoefOfAllTimeHandler : IRequestHandler<MaxCoefOfAllTimeQuery, float?>
    {
        private readonly NewsAggregatorContext _database;

        public MaxCoefOfAllTimeHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<float?> Handle(MaxCoefOfAllTimeQuery query, CancellationToken token)
        {
            var maxCoef = await _database.Articles
                    .MaxAsync(article => article.Coefficient);

            return maxCoef;
        }
    }
}
