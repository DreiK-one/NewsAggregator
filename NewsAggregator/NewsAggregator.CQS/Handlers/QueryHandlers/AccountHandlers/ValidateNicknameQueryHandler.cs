using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class ValidateNicknameQueryHandler : IRequestHandler<ValidateNicknameQuery, bool>
    {
        private readonly NewsAggregatorContext _database;

        public ValidateNicknameQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<bool> Handle(ValidateNicknameQuery query, CancellationToken token)
        {
            return await _database.Users.AsNoTracking()
                .AnyAsync(u => u.NormalizedEmail.Equals(query.Name));
        }
    }
}
