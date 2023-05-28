using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.SourceQueries
{
    public class GetRssUrlsAsyncQuery : IRequest<IEnumerable<RssUrlsFromSourceDto>>
    {
    }
}
