using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.RoleCommands
{
    public class CreateRoleCommand : IRequest<Guid>
    {

        public CreateRoleCommand(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
