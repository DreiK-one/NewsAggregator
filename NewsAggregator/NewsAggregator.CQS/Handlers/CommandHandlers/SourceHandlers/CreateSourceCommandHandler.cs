using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.SourceCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.SourceHandlers
{
    public class CreateSourceCommandHandler : IRequestHandler<CreateSourceCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateSourceCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateSourceCommand command, CancellationToken token)
        {
            var source = _mapper.Map<Source>(command);

            await _database.Sources.AddAsync(source);

            return await _database.SaveChangesAsync(token);
        }
    }
}
