using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.UserRoleCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.UserRoleHandlers
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public UpdateUserRoleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(UpdateUserRoleCommand command, CancellationToken token)
        {
            var userRole = _mapper.Map<UserRole>(command);

            _database.UserRoles.Update(userRole);
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
