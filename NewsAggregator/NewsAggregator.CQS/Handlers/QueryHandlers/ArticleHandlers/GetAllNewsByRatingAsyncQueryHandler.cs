using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllNewsByRatingAsyncQueryHandler : IRequestHandler<GetAllNewsByRatingAsyncQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllNewsByRatingAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllNewsByRatingAsyncQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                    .Where(article => article.Coefficient > 0)
                    .OrderByDescending(article => article.CreationDate)
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();

            return articles;
        }
    }
}
