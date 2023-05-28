using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.SourceQueries
{
    public class GetSourceAsyncQuery : IRequest<SourceDto>
    {
        public GetSourceAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
