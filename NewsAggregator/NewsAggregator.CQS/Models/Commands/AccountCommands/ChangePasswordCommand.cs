using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public ChangePasswordCommand(Guid userId, string password)
        {
            UserId = userId;
            Password = password;
        }

        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
