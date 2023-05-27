using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetUserNicknameByIdAsyncQueryHandler : IRequestHandler<GetUserNicknameByIdAsyncQuery, string?>
    {
        private readonly NewsAggregatorContext _database;

        public GetUserNicknameByIdAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<string?> Handle(GetUserNicknameByIdAsyncQuery query, CancellationToken token)
        {
            var user = await _database.Users.
                FirstOrDefaultAsync(u => u.Id == query.Id);

            return user?.Nickname;
        }
    }
}
