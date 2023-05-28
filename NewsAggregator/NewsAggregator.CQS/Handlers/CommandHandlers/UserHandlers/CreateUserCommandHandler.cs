using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.UserCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.UserHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateUserCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateUserCommand command, CancellationToken token)
        {
            var user = _mapper.Map<User>(command);

            await _database.Users.AddAsync(user);
 
            return await _database.SaveChangesAsync(token);
        }
    }
}
