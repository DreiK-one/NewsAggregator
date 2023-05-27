﻿using NewsAggregetor.CQS.Models.Queries.TokenQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.TokenQueries
{
    public class GetChildTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, RefreshToken>
    {
        private readonly NewsAggregatorContext _database;

        public GetChildTokenQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<RefreshToken> Handle(GetRefreshTokenQuery query, CancellationToken token)
        {
            var refreshToken = await _database.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.ReplacedByToken.Equals(token))
                .FirstOrDefaultAsync(cancellationToken: token);

            return refreshToken;
        }
    }
}
