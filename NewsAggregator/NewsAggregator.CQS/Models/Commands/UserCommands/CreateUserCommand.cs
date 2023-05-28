using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Guid RoleId { get; set; }
    }
}
