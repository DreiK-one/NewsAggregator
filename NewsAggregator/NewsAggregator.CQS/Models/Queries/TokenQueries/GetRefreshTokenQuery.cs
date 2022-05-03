using MediatR;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Models.Queries.TokenQueries
{
    public class GetRefreshTokenQuery : IRequest<RefreshToken>
    {
        public GetRefreshTokenQuery(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
