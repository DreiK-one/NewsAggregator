using AutoMapper;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;
using MediatR;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.CategoryHandlers
{
    public class GetCategoryAsyncQueryHandler : IRequestHandler<GetCategoryAsyncQuery, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetCategoryAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryAsyncQuery query, CancellationToken token)
        {
            var category = await _database.Categories.FindAsync(query.Id);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
