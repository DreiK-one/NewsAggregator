using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserNicknameByIdAsyncQuery : IRequest<string?>
    {
        public GetUserNicknameByIdAsyncQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
