using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticleWithAllNavigationPropertiesQueryHandler : IRequestHandler<GetArticleWithAllNavigationPropertiesQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleWithAllNavigationPropertiesQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleWithAllNavigationPropertiesQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                    .Where(a => a.Id == query.Id)
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                    .ThenInclude(user => user.User)
                    .Select(art => _mapper.Map<ArticleDto>(art))
                    .FirstOrDefaultAsync();

            return article;
        }
    }
}
