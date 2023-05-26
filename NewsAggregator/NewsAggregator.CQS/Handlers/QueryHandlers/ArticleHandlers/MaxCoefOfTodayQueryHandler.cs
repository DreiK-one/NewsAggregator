using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class MaxCoefOfTodayQueryHandler : IRequestHandler<MaxCoefOfTodayQuery, float?>
    {
        private readonly NewsAggregatorContext _database;

        public MaxCoefOfTodayQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<float?> Handle(MaxCoefOfTodayQuery query, CancellationToken token)
        {
            var maxCoef = await _database.Articles
                .Where(article => article.CreationDate.Date == DateTime.Today.Date)
                    .MaxAsync(article => article.Coefficient);

            return maxCoef;
        }
    }
}
