using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetUserIdByEmailAsyncQueryHandler : IRequestHandler<GetUserIdByEmailAsyncQuery, Guid?>
    {
        private readonly NewsAggregatorContext _database;

        public GetUserIdByEmailAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid?> Handle(GetUserIdByEmailAsyncQuery query, CancellationToken token)
        {
            var user = await _database.Users
                    .FirstOrDefaultAsync(u =>
                        u.NormalizedEmail != null &&
                        u.NormalizedEmail == query.Email);

            return user?.Id;
        }
    }
}
