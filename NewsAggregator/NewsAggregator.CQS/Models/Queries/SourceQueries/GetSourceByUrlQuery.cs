using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.SourceQueries
{
    public class GetSourceByUrlQuery : IRequest<Guid>
    {
        public GetSourceByUrlQuery(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}
