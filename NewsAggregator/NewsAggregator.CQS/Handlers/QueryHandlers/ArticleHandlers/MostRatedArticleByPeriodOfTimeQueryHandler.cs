using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;

namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class MostRatedArticleByPeriodOfTimeQueryHandler : IRequestHandler<MostRatedArticleByPeriodOfTimeQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public MostRatedArticleByPeriodOfTimeQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(MostRatedArticleByPeriodOfTimeQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                    .Where(article => article.Coefficient == query.MaxCoef)
                    .Include(source => source.Source)
                    .Include(comments => comments.Comments)
                        .ThenInclude(user => user.User)
                    .FirstOrDefaultAsync();

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
