﻿using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetArticleWithAllNavigationPropertiesQuery : IRequest<ArticleDto>
    {
        public GetArticleWithAllNavigationPropertiesQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
