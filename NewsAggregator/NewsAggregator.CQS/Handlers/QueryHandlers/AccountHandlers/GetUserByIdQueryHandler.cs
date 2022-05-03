using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetUserByIdQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken token)
        {
            var user = await _database.Users
                .AsNoTracking()
                .Where(u => u.Id.Equals(query.Id))
                .Select(art => _mapper.Map<UserDto>(art))
                .FirstOrDefaultAsync(cancellationToken: token);

            return user;
        }
    }
}
