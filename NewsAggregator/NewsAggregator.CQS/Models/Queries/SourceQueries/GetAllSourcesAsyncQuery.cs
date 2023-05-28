using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.SourceQueries
{
    public class GetAllSourcesAsyncQuery : IRequest<IEnumerable<SourceDto>>
    {
    }
}
