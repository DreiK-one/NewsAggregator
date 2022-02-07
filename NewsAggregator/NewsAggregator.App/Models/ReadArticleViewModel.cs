namespace NewsAggregator.App.Models
{
    public class ReadArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

        public SourceModel SourceName { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
