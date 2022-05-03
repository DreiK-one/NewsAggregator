using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetRoleIdByRoleNameQueryHandler : IRequestHandler<GetRoleIdByRoleNameQuery, Guid>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetRoleIdByRoleNameQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(GetRoleIdByRoleNameQuery query, CancellationToken token)
        {
            var roleId = await _database.Roles
                .AsNoTracking()
                .Where(r => r.Name.ToUpper().Equals(query.Name))
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken: token);

            return roleId;
        }
    }
}
