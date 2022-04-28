namespace NewsAggregator.WebAPI.Models.Requests
{
    public class GetArticlesRequest
    {
        public string? Name { get; set; }
        public Guid? SourseId { get; set; }
        public int? Page { get; set; }
    }
}
