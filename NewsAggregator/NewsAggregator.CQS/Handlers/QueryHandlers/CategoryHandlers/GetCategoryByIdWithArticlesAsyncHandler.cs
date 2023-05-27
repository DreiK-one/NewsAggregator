using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetCategoryByIdWithArticlesAsyncHandler : IRequestHandler<GetCategoryByIdWithArticlesAsyncQuery, CategoryWithArticlesDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetCategoryByIdWithArticlesAsyncHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<CategoryWithArticlesDto> Handle(GetCategoryByIdWithArticlesAsyncQuery query, CancellationToken token)
        {
            var category = await _database.Categories
                .Where(c => c.Id.Equals(query.Id))
                .Include(a => a.Articles)
                    .ThenInclude(c => c.Comments)
                .Select(c => _mapper.Map<CategoryWithArticlesDto>(c))
                .FirstOrDefaultAsync(cancellationToken: token);

            return category;
        }
    }
}
