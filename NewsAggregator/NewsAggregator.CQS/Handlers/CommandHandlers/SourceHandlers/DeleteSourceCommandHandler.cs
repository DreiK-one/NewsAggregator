using MediatR;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Commands.SourceCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.SourceHandlers
{
    public class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand, int?>
    {
        private readonly NewsAggregatorContext _database;
        
        public DeleteSourceCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int?> Handle(DeleteSourceCommand command, CancellationToken token)
        {
            var source = await _database.Sources.FindAsync(command.Id);

            _database.Sources.Remove(source);

            return await _database.SaveChangesAsync(token);
        }
    }
}
