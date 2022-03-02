namespace NewsAggregator.App.Models
{
    public class ArticleEditViewModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string SourceUrl { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public float Coefficient { get; set; }

        public SourceModel SourceName { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}
