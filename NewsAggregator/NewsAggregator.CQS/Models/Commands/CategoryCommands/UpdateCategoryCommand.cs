using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.CategoryCommands
{
    public class UpdateCategoryCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
