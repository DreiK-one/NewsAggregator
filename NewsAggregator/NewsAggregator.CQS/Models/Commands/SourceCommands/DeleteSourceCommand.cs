using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.SourceCommands
{
    public class DeleteSourceCommand : IRequest<int?>
    {
        public DeleteSourceCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
