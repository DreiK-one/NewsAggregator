namespace NewsAggregator.App.Models
{
    public class ArticleModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
