using MediatR;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Commands.UserCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.UserHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int?>
    {
        private readonly NewsAggregatorContext _database;
        
        public DeleteUserCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int?> Handle(DeleteUserCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.Id);

            _database.Users.Remove(user);
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
