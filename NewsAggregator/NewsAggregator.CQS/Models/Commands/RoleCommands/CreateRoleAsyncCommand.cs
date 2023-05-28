using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.RoleCommands
{
    public class CreateRoleAsyncCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
