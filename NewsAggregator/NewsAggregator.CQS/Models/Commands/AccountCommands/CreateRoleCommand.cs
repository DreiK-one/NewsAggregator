using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class CreateRoleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}
