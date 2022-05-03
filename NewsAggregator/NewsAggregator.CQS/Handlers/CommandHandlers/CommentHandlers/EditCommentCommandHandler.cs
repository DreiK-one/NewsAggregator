using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;

namespace NewsAggregetor.CQS.Handlers.CommandHandlers.CommentHandlers
{
    public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public EditCommentCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EditCommentCommand command, CancellationToken token)
        {
            var comment = _mapper.Map<Comment>(command);

            _database.Comments.Update(comment);
            await _database.SaveChangesAsync(token);

            return true;
        }
    }
}
