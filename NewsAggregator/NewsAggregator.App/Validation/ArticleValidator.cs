using FluentValidation;
using NewsAggregator.App.Models;


namespace NewsAggregator.App.Validation
{
    public class ArticleValidator : AbstractValidator<CreateOrEditArticleViewModel>
    {
        public ArticleValidator()
        {
            RuleFor(article => article.Title).NotNull()
                .MinimumLength(4).WithMessage("Minimum length of this field is 4");

            RuleFor(article => article.Description).NotNull();

            RuleFor(article => article.Body).NotNull();

            RuleFor(article => article.SourceUrl).NotNull();

            RuleFor(article => article.Image).NotNull();

            RuleFor(article => article.Coefficient).NotNull();
        }
    }
}
