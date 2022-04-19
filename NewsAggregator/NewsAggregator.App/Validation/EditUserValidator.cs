using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Validation
{
    public class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public EditUserValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(user => user.Nickname)
                .NotNull().WithMessage("Nickname is required")
                .MinimumLength(4).WithMessage("Minimum length of nickname is 4");

            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
