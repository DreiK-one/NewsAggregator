using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using MediatR;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CommentHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public DeleteCommentCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteCommentCommand command, CancellationToken token)
        {
            var comment = await _database.Comments.FindAsync(command.Id);
            _database.Comments.Remove(comment);
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
