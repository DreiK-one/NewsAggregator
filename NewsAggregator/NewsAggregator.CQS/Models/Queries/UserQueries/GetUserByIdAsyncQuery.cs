using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.UserQueries
{
    public class GetUserByIdAsyncQuery : IRequest<UserDto>
    {
        public GetUserByIdAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
