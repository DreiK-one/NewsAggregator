using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;

namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticleWithoutRatingQueryHandler : IRequestHandler<GetArticleWithoutRatingQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleWithoutRatingQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleWithoutRatingQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                    .Where(article => article.Coefficient.Equals(null) &&
                        !string.IsNullOrWhiteSpace(article.Body))
                    .Take(1)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .FirstOrDefaultAsync();

            return article;
        }
    }
}
