using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.UserRoleCommands
{
    public class UpdateUserRoleCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
