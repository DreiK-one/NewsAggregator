using FluentValidation;
using NewsAggregator.WebAPI.Models.Requests;


namespace NewsAggregator.WebAPI.Validators
{
    public class CommentValidator : AbstractValidator<CreateCommentRequest>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Text)
               .NotNull();
            RuleFor(comment => comment.UserId)
               .NotNull();
            RuleFor(comment => comment.ArticleId)
               .NotNull();
            RuleFor(comment => comment.CreationDate)
               .NotNull();
        }
    }
}
