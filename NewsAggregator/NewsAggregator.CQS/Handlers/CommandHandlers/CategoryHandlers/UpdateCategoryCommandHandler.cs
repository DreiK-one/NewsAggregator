using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.CategoryCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CategoryHandlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public UpdateCategoryCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(UpdateCategoryCommand command, CancellationToken token)
        {
            var category = _mapper.Map<Category>(command);

            _database.Categories.Update(category);
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
