using MediatR;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Models.Queries.TokenQueries
{
    public class GetChildTokenQuery : IRequest<RefreshToken>
    {
        public GetChildTokenQuery(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
