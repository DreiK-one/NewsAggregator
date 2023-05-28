using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.UserCommands
{
    public class CreateUserRoleCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
