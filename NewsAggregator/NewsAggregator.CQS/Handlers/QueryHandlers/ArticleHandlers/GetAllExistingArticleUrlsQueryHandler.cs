using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllExistingArticleUrlsQueryHandler : IRequestHandler<GetAllExistingArticleUrlsQuery, List<string>>
    {
        private readonly NewsAggregatorContext _database;

        public GetAllExistingArticleUrlsQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<List<string>> Handle(GetAllExistingArticleUrlsQuery query, CancellationToken token)
        {
            var urls = await _database.Articles
                    .Select(article => article.SourceUrl)
                    .ToListAsync();

            return urls;
        }
    }
}
