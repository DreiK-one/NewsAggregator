namespace NewsAggregator.Core.DTOs
{
    public class SourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string RssUrl { get; set; }
    }
}
