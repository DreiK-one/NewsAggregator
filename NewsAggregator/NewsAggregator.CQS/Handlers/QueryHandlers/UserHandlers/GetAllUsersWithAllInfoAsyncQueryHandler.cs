using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.UserQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.UserHandlers
{
    public class GetAllUsersWithAllInfoAsyncQueryHandler : IRequestHandler<GetAllUsersWithAllInfoAsyncQuery, IEnumerable<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllUsersWithAllInfoAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersWithAllInfoAsyncQuery query, CancellationToken token)
        {
            return await _database.Users
                .Include(userRoles => userRoles.UserRoles)
                .ThenInclude(role => role.Role)
                .Include(comments => comments.Comments)
                .ThenInclude(articles => articles.Article)
                .Select(users => _mapper.Map<UserDto>(users))
                .ToListAsync();
        }
    }
}
