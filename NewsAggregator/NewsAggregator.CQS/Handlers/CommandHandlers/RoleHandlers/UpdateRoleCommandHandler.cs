using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.RoleCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.RoleHandlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public UpdateRoleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(UpdateRoleCommand command, CancellationToken token)
        {
            var role = _mapper.Map<Role>(command);

            _database.Roles.Update(role);
            
            return await _database.SaveChangesAsync(token);
        }
    }
}
