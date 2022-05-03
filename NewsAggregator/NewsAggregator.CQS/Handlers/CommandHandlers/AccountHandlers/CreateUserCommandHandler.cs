using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateUserCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateUserCommand command, CancellationToken token)
        {
            var user = _mapper.Map<User>(command);

            await _database.Users.AddAsync(user);
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
