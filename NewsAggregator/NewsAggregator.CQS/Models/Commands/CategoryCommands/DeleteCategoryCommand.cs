using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.CategoryCommands
{
    public class DeleteCategoryCommand : IRequest<int?>
    {
        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
