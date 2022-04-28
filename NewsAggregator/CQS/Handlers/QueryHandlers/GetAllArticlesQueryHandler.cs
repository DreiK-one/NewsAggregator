using AutoMapper;
using CQS.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Handlers.QueryHandlers
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
            .OrderBy(article => article.CreationDate)
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToArrayAsync(cancellationToken: token);

            return articles;
        }
    }
}
