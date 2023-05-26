using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.ArticleCommands
{
    public class DeleteArticleCommand : IRequest<int?>
    {
        public DeleteArticleCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
