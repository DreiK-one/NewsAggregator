using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.UserQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.UserHandlers
{
    public class GetUserByIdAsyncQueryHandler : IRequestHandler<GetUserByIdAsyncQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetUserByIdAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdAsyncQuery query, CancellationToken token)
        {
            var user = await _database.Users
                .Where(u => u.Id == query.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
