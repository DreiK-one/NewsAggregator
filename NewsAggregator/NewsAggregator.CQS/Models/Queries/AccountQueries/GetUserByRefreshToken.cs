using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserByRefreshToken : IRequest<UserDto>
    {
        public GetUserByRefreshToken(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}
