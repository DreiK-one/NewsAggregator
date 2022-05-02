using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllArticlesQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .OrderByDescending(art => art.CreationDate)
                .Select(art => _mapper.Map<ArticleDto>(art))
                .ToListAsync(cancellationToken: token);

            return articles;
        }
    }
}
