namespace NewsAggregator.App.Models
{
    public class SourceModel : BaseModel
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string RssUrl { get; set; }
    }
}
