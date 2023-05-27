using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class UpdateNicknameCommandHandler : IRequestHandler<UpdateNicknameCommand, int>
    {
        private readonly NewsAggregatorContext _database;
        
        public UpdateNicknameCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<int> Handle(UpdateNicknameCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.UserId);
            user.Nickname = command.Nickname;
            user.NormalizedNickname = command.NormalizedNickname;
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
