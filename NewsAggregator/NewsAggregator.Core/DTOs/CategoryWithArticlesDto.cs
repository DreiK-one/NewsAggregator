namespace NewsAggregator.Core.DTOs
{
    public class CategoryWithArticlesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<ArticleDto> Articles { get; set; }
    }
}
