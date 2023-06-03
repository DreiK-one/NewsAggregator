using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticleWithEmptyBodyAndTitleQueryHandler : IRequestHandler<GetArticleWithEmptyBodyAndTitleQuery, Article>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleWithEmptyBodyAndTitleQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Article> Handle(GetArticleWithEmptyBodyAndTitleQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                    .FirstOrDefaultAsync(a => string.IsNullOrWhiteSpace(a.Body)
                        && string.IsNullOrWhiteSpace(a.Title));

            return article;
        }
    }
}
