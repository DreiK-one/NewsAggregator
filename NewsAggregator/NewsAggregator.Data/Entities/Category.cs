namespace NewsAggregator.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual IEnumerable<Article> Articles { get; set; }
    }
}
