using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.CategoryCommands
{
    public class CreateCategoryCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
