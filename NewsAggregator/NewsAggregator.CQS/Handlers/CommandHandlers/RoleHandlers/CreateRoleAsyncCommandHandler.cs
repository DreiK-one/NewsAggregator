using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Commands.RoleCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.RoleHandlers
{
    public class CreateRoleAsyncCommandHandler : IRequestHandler<CreateRoleAsyncCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateRoleAsyncCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateRoleAsyncCommand command, CancellationToken token)
        {
            var role = _mapper.Map<Role>(command);

            await _database.Roles.AddAsync(role);

            return await _database.SaveChangesAsync(token);
        }
    }
}
