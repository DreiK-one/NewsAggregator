using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
