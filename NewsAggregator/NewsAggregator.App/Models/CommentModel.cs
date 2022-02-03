namespace NewsAggregator.App.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        //todo UserName
    }
}
