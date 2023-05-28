using MediatR;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Commands.RoleCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.RoleHandlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int?>
    {
        private readonly NewsAggregatorContext _database;
        
        public DeleteRoleCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int?> Handle(DeleteRoleCommand command, CancellationToken token)
        {
            var role = await _database.Roles.FindAsync(command.Id);

            _database.Roles.Remove(role);

            return await _database.SaveChangesAsync(token);
        }
    }
}
