namespace NewsAggregator.App.Models
{
    public class CategoryWithArticlesViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<ArticleModel> Articles { get; set; }
    }
}
