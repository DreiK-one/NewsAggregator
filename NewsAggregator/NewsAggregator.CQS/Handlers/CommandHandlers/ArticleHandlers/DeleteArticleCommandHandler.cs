using AutoMapper;
using MediatR;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.ArticleHandlers
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public DeleteArticleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(DeleteArticleCommand command, CancellationToken token)
        {
            var article = await _database.Articles.FindAsync(command.Id);

            _database.Articles.Remove(article);

            return await _database.SaveChangesAsync(token); ;
        }
    }
}
