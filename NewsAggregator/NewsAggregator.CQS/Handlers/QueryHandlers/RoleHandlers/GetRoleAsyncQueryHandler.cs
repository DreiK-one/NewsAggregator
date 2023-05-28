using AutoMapper;
using MediatR;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.RoleHandlers
{
    public class GetRoleAsyncQueryHandler : IRequestHandler<GetRoleAsyncQuery, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetRoleAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(GetRoleAsyncQuery query, CancellationToken token)
        {
            var role = await _database.Roles.FindAsync(query.Id);

            return _mapper.Map<RoleDto>(role);
        }
    }
}
