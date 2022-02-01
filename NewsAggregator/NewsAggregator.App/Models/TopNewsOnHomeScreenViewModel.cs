namespace NewsAggregator.App.Models
{
    public class TopNewsOnHomeScreenViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }

    }
}
