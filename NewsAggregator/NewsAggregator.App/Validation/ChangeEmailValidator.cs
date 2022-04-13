using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Validation
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmailViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public ChangeEmailValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(model => model.CurrentEmail)
                .NotNull()
                .EmailAddress().WithMessage("Invalid email format")
                .Must(_validationMethods.CheckIsEmailDoesntExists).WithMessage("User with this email is doesn't exist")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Email can't contain word \"admin\"");

            RuleFor(model => model.NewEmail)
                .NotNull()
                .EmailAddress().WithMessage("Invalid email format")
                .Must(_validationMethods.CheckIsEmailExists).WithMessage("User with this email is already exists")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Nickname can't contain word \"admin\""); ;

            RuleFor(model => model.ConfirmEmail)
                .Equal(model => model.NewEmail).WithMessage("Emails do not match");

        }
    }
}
