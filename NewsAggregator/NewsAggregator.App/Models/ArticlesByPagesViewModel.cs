namespace NewsAggregator.App.Models
{
    public class ArticlesByPagesViewModel
    {
        public List<AllNewsOnHomeScreenViewModel> NewsList { get; set; }
        public Pager Pager { get; set; }
    }
}
