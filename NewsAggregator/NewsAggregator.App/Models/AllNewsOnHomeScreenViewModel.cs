namespace NewsAggregator.App.Models
{
    public class AllNewsOnHomeScreenViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
        public string Image { get; set; }
		public DateTime CreationDate { get; set; }
    }
}
