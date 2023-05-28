using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Queries.UserRoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.UserRoleHandlers
{
    public class GetUserRoleByUserIdQueryHandler : IRequestHandler<GetUserRoleByUserIdQuery, UserRole>
    {
        private readonly NewsAggregatorContext _database;

        public GetUserRoleByUserIdQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<UserRole> Handle(GetUserRoleByUserIdQuery query, CancellationToken token)
        {
            return await _database.UserRoles
                        .FirstOrDefaultAsync(ur => ur.UserId == query.UserId);
        }
    }
}
