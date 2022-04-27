namespace NewsAggregator.WebAPI.Models.Requests
{
    public class GetArticlesRequest
    {
        public string CategoryName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Rating { get; set; }
    }
}
