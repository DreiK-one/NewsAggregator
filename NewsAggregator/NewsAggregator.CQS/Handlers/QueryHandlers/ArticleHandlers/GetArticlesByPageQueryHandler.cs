using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticlesByPageQueryHandler : IRequestHandler<GetArticlesByPageQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
  
        public GetArticlesByPageQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesByPageQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .OrderByDescending(article => article.CreationDate)
                .Skip(query.PageNumber * query.PageSize)
                .Take(query.PageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync(cancellationToken: token);

            return articles;
        }
    }
}
