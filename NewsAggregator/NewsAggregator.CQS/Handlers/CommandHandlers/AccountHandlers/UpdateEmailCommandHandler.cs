using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, int>
    {
        private readonly NewsAggregatorContext _database;
        
        public UpdateEmailCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int> Handle(UpdateEmailCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.UserId);
            user.Email = command.Email;
            user.NormalizedEmail = command.NormalizedEmail;
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
