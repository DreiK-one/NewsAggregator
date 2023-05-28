using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.SourceCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.SourceHandlers
{
    public class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public UpdateSourceCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(UpdateSourceCommand command, CancellationToken token)
        {
            var source = _mapper.Map<Source>(command);

            _database.Sources.Update(source);
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
