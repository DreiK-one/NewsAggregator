using AutoMapper;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
using MediatR;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Handlers.CommandHandlers.ArticleHandlers
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, int?>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;
        
        public CreateArticleCommandHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateArticleCommand command, CancellationToken token)
        {
            var article = _mapper.Map<Article>(command);

            await _database.Articles.AddAsync(article);
 
            return await _database.SaveChangesAsync(token);
        }
    }
}
