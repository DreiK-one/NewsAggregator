using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.ArticleHandlers
{
    public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public EditArticleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(EditArticleCommand command, CancellationToken token)
        {
            var article = _mapper.Map<Article>(command);

            _database.Articles.Update(article);
 
            return await _database.SaveChangesAsync(token);
        }
    }
}
