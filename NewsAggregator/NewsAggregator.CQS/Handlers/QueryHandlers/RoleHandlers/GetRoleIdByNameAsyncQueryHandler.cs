using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.RoleHandlers
{
    public class GetRoleIdByNameAsyncQueryHandler : IRequestHandler<GetRoleIdByNameAsyncQuery, Guid>
    {
        private readonly NewsAggregatorContext _database;

        public GetRoleIdByNameAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid> Handle(GetRoleIdByNameAsyncQuery query, CancellationToken token)
        {
            var role = await _database.Roles
                   .FirstOrDefaultAsync(role => role.Name == query.Name);

            return role.Id;
        }
    }
}
