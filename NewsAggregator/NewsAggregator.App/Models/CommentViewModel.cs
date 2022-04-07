namespace NewsAggregator.App.Models
{
    public class CommentViewModel : BaseModel
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public UserModel User { get; set; }
    }
}
