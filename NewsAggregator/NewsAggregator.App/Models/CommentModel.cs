using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class CommentModel : BaseModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? UserId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ReturnUrl { get; set; }
    }
}
