using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByUrlAsyncQuery : IRequest<Guid>
    {
        public GetCategoryByUrlAsyncQuery(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}
