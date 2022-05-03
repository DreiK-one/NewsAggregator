using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;

namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllPositiveArticlesQueryHandler : IRequestHandler<GetAllPositiveArticlesQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllPositiveArticlesQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllPositiveArticlesQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .Where(c => c.Coefficient > 0)
                .OrderByDescending(art => art.CreationDate)
                .Select(art => _mapper.Map<ArticleDto>(art))
                .ToListAsync(cancellationToken: token);

            return articles;
        }
    }
}
