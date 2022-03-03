namespace NewsAggregator.App.Models
{
    public class AllNewsOnHomeScreenViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
        public string Body { get; set; }
        public string SourceUrl { get; set; }
        public string Image { get; set; }
		public DateTime CreationDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SourceId { get; set; }
    }
}
