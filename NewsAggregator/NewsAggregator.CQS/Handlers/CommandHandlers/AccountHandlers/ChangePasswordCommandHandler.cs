using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public ChangePasswordCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.UserId);
            user.PasswordHash = command.Password;
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
