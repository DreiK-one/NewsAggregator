using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
