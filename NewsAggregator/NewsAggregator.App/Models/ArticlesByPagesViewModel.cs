namespace NewsAggregator.App.Models
{
    public class ArticlesByPagesViewModel
    {
        public List<AllNewsOnHomeScreenViewModel> NewsList { get; set; }
        public int PageAmount { get; set; }
    }
}
