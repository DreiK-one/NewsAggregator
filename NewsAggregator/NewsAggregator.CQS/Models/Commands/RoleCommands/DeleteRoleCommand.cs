using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.RoleCommands
{
    public class DeleteRoleCommand : IRequest<int?>
    {
        public DeleteRoleCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
