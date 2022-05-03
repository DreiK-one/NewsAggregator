using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class ValidateEmailQuery : IRequest<bool>
    {
        public ValidateEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
