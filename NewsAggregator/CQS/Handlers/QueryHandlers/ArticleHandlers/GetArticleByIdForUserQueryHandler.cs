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
    public class GetArticleByIdForUserQueryHandler : IRequestHandler<GetArticleByIdForUserQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleByIdForUserQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdForUserQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                .Where(art => art.Coefficient > 0 && art.Id.Equals(query.Id))
                .Select(art => _mapper.Map<ArticleDto>(art))
                .FirstOrDefaultAsync(cancellationToken: token);

            return article;
        }
    }
}
