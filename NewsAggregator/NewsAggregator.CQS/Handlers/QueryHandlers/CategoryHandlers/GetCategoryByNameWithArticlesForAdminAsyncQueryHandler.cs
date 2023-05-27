using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetCategoryByNameWithArticlesForAdminAsyncQueryHandler : IRequestHandler<GetCategoryByNameWithArticlesForAdminAsyncQuery, CategoryWithArticlesDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetCategoryByNameWithArticlesForAdminAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<CategoryWithArticlesDto> Handle(GetCategoryByNameWithArticlesForAdminAsyncQuery query, CancellationToken token)
        {
            var category = await _database.Categories
                    .Where(category => category.Name == query.Name)
                    .Include(article => article.Articles
                        .OrderByDescending(article => article.CreationDate))
                    .FirstOrDefaultAsync();

            return _mapper.Map<CategoryWithArticlesDto>(category);
        }
    }
}
