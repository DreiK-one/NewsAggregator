using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.UserCommands
{
    public class DeleteUserCommand : IRequest<int?>
    {
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
