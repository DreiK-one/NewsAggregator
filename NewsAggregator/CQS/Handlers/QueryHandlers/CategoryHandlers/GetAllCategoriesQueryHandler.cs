using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryWithArticlesDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetAllCategoriesQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryWithArticlesDto>> Handle(GetAllCategoriesQuery query, CancellationToken token)
        {
            var categories = await _database.Categories
                .Include(a => a.Articles)
                    .ThenInclude(c => c.Comments)
                .Select(c => _mapper.Map<CategoryWithArticlesDto>(c))
                .ToListAsync(cancellationToken: token);

            return categories;
        }
    }
}
