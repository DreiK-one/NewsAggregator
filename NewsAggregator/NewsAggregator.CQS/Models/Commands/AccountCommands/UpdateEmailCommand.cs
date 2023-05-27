using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class UpdateEmailCommand : IRequest<int>
    {
        public UpdateEmailCommand(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
    }
}
