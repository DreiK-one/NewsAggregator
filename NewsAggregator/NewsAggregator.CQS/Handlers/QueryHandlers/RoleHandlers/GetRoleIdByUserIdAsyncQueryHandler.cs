using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.RoleHandlers
{
    public class GetRoleIdByUserIdAsyncQueryHandler : IRequestHandler<GetRoleIdByUserIdAsyncQuery, Guid>
    {
        private readonly NewsAggregatorContext _database;

        public GetRoleIdByUserIdAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid> Handle(GetRoleIdByUserIdAsyncQuery query, CancellationToken token)
        {
            return await _database.UserRoles
                    .Where(userId => userId.UserId == query.Id)
                    .Select(roleId => roleId.RoleId)
                    .FirstOrDefaultAsync();
        }
    }
}
