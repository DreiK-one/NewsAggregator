using FluentValidation;
using NewsAggregator.WebAPI.Models.Requests;


namespace NewsAggregator.WebAPI.Validators
{
    public class CommentValidator : AbstractValidator<CreateCommentRequest>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Text)
               .NotEmpty().WithMessage("Comment text is required!");
            RuleFor(comment => comment.CreationDate)
               .NotEmpty().WithMessage("Date is required!");
            RuleFor(comment => comment.UserId)
               .NotEmpty();
            RuleFor(comment => comment.ArticleId)
               .NotEmpty();
        }
    }
}
