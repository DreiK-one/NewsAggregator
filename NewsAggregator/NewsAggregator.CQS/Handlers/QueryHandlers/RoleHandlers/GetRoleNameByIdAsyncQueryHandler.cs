using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.RoleHandlers
{
    public class GetRoleNameByIdAsyncQueryHandler : IRequestHandler<GetRoleNameByIdAsyncQuery, string>
    {
        private readonly NewsAggregatorContext _database;

        public GetRoleNameByIdAsyncQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<string> Handle(GetRoleNameByIdAsyncQuery query, CancellationToken token)
        {
            var role = await _database.Roles.FirstOrDefaultAsync(r => r.Id == query.Id);

            return role.Name;
        }
    }
}
