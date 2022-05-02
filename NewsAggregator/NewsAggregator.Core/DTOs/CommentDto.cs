namespace NewsAggregator.Core.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public UserDto User { get; set; }

        public ArticleDto Article { get; set; }


    }
}
