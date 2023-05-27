using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.CategoryCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CategoryHandlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateCategoryCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateCategoryCommand command, CancellationToken token)
        {
            var category = _mapper.Map<Category>(command);

            await _database.Categories.AddAsync(category);

            return await _database.SaveChangesAsync(token);
        }
    }
}
