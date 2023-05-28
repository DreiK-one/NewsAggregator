using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.UserQueries
{
    public class GetAllUsersWithAllInfoAsyncQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
