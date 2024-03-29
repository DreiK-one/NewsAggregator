﻿using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly NewsAggregatorContext _database;
        
        public ChangePasswordCommandHandler(NewsAggregatorContext database)
        {
            _database = database;
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
