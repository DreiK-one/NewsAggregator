using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class MaxCoefOfTheMonthQueryHandler : IRequestHandler<MaxCoefOfTheMonthQuery, float?>
    {
        private readonly NewsAggregatorContext _database;

        public MaxCoefOfTheMonthQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<float?> Handle(MaxCoefOfTheMonthQuery query, CancellationToken token)
        {
            var maxCoef = await _database.Articles
                .Where(article => article.CreationDate.Date.Month == DateTime.Today.Date.Month)
                    .MaxAsync(article => article.Coefficient);

            return maxCoef;
        }
    }
}
