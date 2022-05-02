using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CommentHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateCommentCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateCommentCommand command, CancellationToken token)
        {
            var comment = _mapper.Map<Comment>(command.Comment);

            await _database.Comments.AddAsync(comment);
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
