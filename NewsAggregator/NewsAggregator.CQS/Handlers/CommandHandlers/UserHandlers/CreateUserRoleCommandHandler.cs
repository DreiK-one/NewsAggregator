using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.UserCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.UserHandlers
{
    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateUserRoleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateUserRoleCommand command, CancellationToken token)
        {
            var userRole = _mapper.Map<UserRole>(command);

            await _database.UserRoles.AddAsync(userRole);
 
            return await _database.SaveChangesAsync(token);
        }
    }
}
