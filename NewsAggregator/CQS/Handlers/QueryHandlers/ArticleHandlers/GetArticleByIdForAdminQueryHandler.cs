﻿using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.ArticleHandlers
{
    public class GetArticleByIdForAdminQueryHandler : IRequestHandler<GetArticleByIdForAdminQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetArticleByIdForAdminQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdForAdminQuery query, CancellationToken token)
        {
            var article = await _database.Articles
                .Where(art => art.Id.Equals(query.Id))
                .Select(art => _mapper.Map<ArticleDto>(art))
                .FirstOrDefaultAsync(cancellationToken: token);

            return article;
        }
    }
}
