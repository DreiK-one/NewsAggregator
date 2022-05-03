using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class CreateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string NormalizedNickname { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
