using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllArticlesForUserQueryHandler : IRequestHandler<GetAllArticlesForUserQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllArticlesForUserQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesForUserQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .Where(art => art.Coefficient > 0)
                .OrderByDescending(art => art.CreationDate)
                .Select(art => _mapper.Map<ArticleDto>(art))
                .ToListAsync(cancellationToken: token);

            return articles;
        }
    }
}
