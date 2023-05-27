using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserIdByNicknameAsyncQuery : IRequest<Guid>
    {
        public GetUserIdByNicknameAsyncQuery(string nickname)
        {
            Nickname = nickname.ToUpperInvariant();
        }

        public string Nickname { get; set; }
    }
}
