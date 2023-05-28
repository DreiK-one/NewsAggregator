using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.SourceQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.SourceHandlers
{
    public class GetRssUrlsAsyncQueryHandler : IRequestHandler<GetRssUrlsAsyncQuery, IEnumerable<RssUrlsFromSourceDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _database;

        public GetRssUrlsAsyncQueryHandler(NewsAggregatorContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RssUrlsFromSourceDto>> Handle(GetRssUrlsAsyncQuery query, CancellationToken token)
        {
            return await _database.Sources
                    .Select(source => _mapper.Map<RssUrlsFromSourceDto>(source))
                    .ToListAsync();
        }
    }
}
