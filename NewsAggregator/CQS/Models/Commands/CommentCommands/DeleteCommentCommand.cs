using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.CommentCommands
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public DeleteCommentCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
