using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.SourceQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.SourceHandlers
{
    public class GetSourceAsyncQueryHandler : IRequestHandler<GetSourceAsyncQuery, SourceDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetSourceAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<SourceDto> Handle(GetSourceAsyncQuery query, CancellationToken token)
        {
            var source = await _database.Sources
                .Where(u => u.Id == query.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<SourceDto>(source);
        }
    }
}
