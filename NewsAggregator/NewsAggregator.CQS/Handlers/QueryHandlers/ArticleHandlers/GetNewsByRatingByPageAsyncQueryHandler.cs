using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.Helpers;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetNewsByRatingByPageAsyncQueryHandler : IRequestHandler<GetNewsByRatingByPageAsyncQuery, IEnumerable<ArticleDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
  
        public GetNewsByRatingByPageAsyncQueryHandler(NewsAggregatorContext database, 
            IMapper mapper, IConfiguration configuration)
        {
            _database = database;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetNewsByRatingByPageAsyncQuery query, CancellationToken token)
        {
            var pageSize = Convert.ToInt32(
                    _configuration[Variables.ConfigurationFields.PageSize]);

            return await _database.Articles
                .Where(article => article.Coefficient > 0)
                .OrderByDescending(article => article.CreationDate)
                .Skip(query.Page * pageSize)
                .Take(pageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToListAsync();
        }
    }
}
