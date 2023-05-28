using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.RoleQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.RoleHandlers
{
    public class GetAllRolesAsyncQueryHandler : IRequestHandler<GetAllRolesAsyncQuery, IEnumerable<RoleDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllRolesAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesAsyncQuery query, CancellationToken token)
        {
            return await _database.Roles
                .Select(role => _mapper.Map<RoleDto>(role))
                .ToListAsync();
        }
    }
}
