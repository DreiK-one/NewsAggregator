using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class ValidateNicknameQuery : IRequest<bool>
    {
        public ValidateNicknameQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
