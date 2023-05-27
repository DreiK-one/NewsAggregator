using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class SetPasswordCommand : IRequest<int>
    {
        public SetPasswordCommand(Guid userId, string password)
        {
            UserId = userId;
            Password = password;
        }

        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
