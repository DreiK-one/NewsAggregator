using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.UserRoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.UserRoleHandlers
{
    public class GetUserRoleIdsByUserIdQueryHandler : IRequestHandler<GetUserRoleIdsByUserIdQuery, IEnumerable<Guid>>
    {
        private readonly NewsAggregatorContext _database;

        public GetUserRoleIdsByUserIdQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Guid>> Handle(GetUserRoleIdsByUserIdQuery query, CancellationToken token)
        {
            var userRoles = await _database.UserRoles
                .Where(u => u.Id == query.UserId).Select(r => r.RoleId).ToListAsync();

            return userRoles.AsEnumerable();

        }
    }
}
