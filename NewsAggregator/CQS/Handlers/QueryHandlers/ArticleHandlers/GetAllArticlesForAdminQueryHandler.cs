using AutoMapper;
using CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetAllArticlesForAdminQueryHandler : IRequestHandler<GetAllArticlesForAdminQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllArticlesForAdminQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesForAdminQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .OrderByDescending(art => art.CreationDate)
                .Select(art => _mapper.Map<ArticleDto>(art))
                .ToListAsync(cancellationToken: token);

            return articles;
        }
    }
}
