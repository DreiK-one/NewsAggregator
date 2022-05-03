using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.AccountHandlers
{
    public class GetUserByRefreshTokenHandler : IRequestHandler<GetUserByRefreshToken, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetUserByRefreshTokenHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByRefreshToken query, CancellationToken token)
        {
            var user = await _database.RefreshTokens
                .AsNoTracking()
                .Where(t => t.Token.Equals(query.Token))
                    .Include(u => u.User)
                .FirstOrDefaultAsync(token);

            return _mapper.Map<UserDto>(user.User);
        }
    }
}
