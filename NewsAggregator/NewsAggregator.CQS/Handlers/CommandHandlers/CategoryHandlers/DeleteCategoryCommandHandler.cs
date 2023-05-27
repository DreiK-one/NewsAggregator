using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Commands.CategoryCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CategoryHandlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public DeleteCategoryCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(DeleteCategoryCommand command, CancellationToken token)
        {
            var category = await _database.Categories.FindAsync(command.Id);

            _database.Categories.Remove(category);

            return await _database.SaveChangesAsync(token);
        }
    }
}
