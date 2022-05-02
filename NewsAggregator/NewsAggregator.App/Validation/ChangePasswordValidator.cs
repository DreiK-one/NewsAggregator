using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;


namespace NewsAggregator.App.Validation
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public ChangePasswordValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(model => model.CurrentPassword)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8");
                

            RuleFor(model => model.NewPassword)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8")
                .Must(_validationMethods.HasLowerCase)
                    .WithMessage("Password must contain at least one lowercase letter")
                .Must(_validationMethods.HasUpperCase)
                    .WithMessage("Password must contain at least one uppercase letter")
                .Must(_validationMethods.HasDigit)
                    .WithMessage("Password must contain at least one digit")
                .Must(_validationMethods.HasSymbol)
                    .WithMessage("Password must contain at least one spacial symbol");

            RuleFor(account => account.ConfirmPassword)
                .Equal(account => account.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
