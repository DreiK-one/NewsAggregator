using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetUserIdByNicknameAsyncQueryHandler : IRequestHandler<GetUserIdByNicknameAsyncQuery, Guid>
    {
        private readonly NewsAggregatorContext _database;

        public GetUserIdByNicknameAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid> Handle(GetUserIdByNicknameAsyncQuery query, CancellationToken token)
        {
            var user = await _database.Users
                    .FirstOrDefaultAsync(user => user.NormalizedNickname != null &&
                        user.NormalizedNickname == query.Nickname);

            if (user == null)
            {
                return Guid.Empty;
            }

            return user.Id;
        }
    }
}
