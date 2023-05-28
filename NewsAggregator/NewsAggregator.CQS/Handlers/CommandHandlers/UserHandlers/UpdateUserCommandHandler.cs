using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.UserCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.UserHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public UpdateUserCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(UpdateUserCommand command, CancellationToken token)
        {
            var user = _mapper.Map<User>(command);

            _database.Users.Update(user);
 
            return await _database.SaveChangesAsync(token);
        }
    }
}
