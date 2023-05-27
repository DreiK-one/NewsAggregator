using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, int>
    {
        private readonly NewsAggregatorContext _database;
        
        public SetPasswordCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int> Handle(SetPasswordCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.UserId);
            user.PasswordHash = command.Password;
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
