using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticleWithAllNavigationPropertiesByRatingQueryHandler : IRequestHandler<GetArticleWithAllNavigationPropertiesByRatingQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleWithAllNavigationPropertiesByRatingQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleWithAllNavigationPropertiesByRatingQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                    .Where(a => a.Id == query.Id && a.Coefficient > 0)
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                    .ThenInclude(user => user.User)
                    .Select(art => _mapper.Map<ArticleDto>(art))
                    .FirstOrDefaultAsync();

            return article;
        }
    }
}
