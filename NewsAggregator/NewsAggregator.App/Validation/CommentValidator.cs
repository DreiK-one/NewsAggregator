using FluentValidation;
using NewsAggregator.App.Models;

namespace NewsAggregator.App.Validation
{
    public class CommentValidator : AbstractValidator<CommentModel>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Text)
               .NotNull().WithMessage("Comment can not be null");
        }
    }
}
