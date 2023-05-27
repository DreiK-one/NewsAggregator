using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetUserIdByEmailAsyncQuery : IRequest<Guid?>
    {
        public GetUserIdByEmailAsyncQuery(string email)
        {
            Email = email.ToUpperInvariant();
        }

        public string Email { get; set; }
    }
}
