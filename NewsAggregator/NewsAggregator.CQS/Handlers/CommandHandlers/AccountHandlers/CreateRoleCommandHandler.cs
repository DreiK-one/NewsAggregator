using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Handlers.CommandHandlers.AccountCommands
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateRoleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateRoleCommand command, CancellationToken token)
        {
            var userRole = _mapper.Map<UserRole>(command);

            await _database.UserRoles.AddAsync(userRole);
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
