using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregetor.CQS.Models.Queries.SourceQueries;


namespace NewsAggregetor.CQS.Handlers.QueryHandlers.SourceHandlers
{
    public class GetSourceByUrlQueryHandler : IRequestHandler<GetSourceByUrlQuery, Guid>
    {
        private readonly NewsAggregatorContext _database;

        public GetSourceByUrlQueryHandler(NewsAggregatorContext database)
        {
            _database = database;
        }

        public async Task<Guid> Handle(GetSourceByUrlQuery query, CancellationToken token)
        {
            var domain = string.Join(".",
                new Uri(query.Url).Host
                .Split('.')
                .TakeLast(2)
                .ToList());

            return (await _database.Sources
                 .FirstOrDefaultAsync(source => source.BaseUrl == domain))?.Id ?? Guid.Empty;
        }
    }
}
