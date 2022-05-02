using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetPositiveArticlesByPageQueryHandler : IRequestHandler<GetPositiveArticlesByPageQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
  
        public GetPositiveArticlesByPageQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetPositiveArticlesByPageQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .Where(a => a.Coefficient > 0)
                .OrderByDescending(article => article.CreationDate)
                .Skip(query.PageNumber * query.PageSize)
                .Take(query.PageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync(cancellationToken: token);

            return articles;
        }
    }
}
