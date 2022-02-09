namespace NewsAggregator.App.Models
{
    public class CommentModel : BaseModel
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public UserModel User { get; set; }
    }
}
