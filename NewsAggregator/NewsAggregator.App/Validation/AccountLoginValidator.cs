using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Validation
{
    public class AccountLoginValidator : AbstractValidator<AccountLoginModel>
    {
        private readonly IValidationMethods _validationMethods;
        public AccountLoginValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(account => account.Email)
               .NotNull().WithMessage("Email is required")
               .EmailAddress().WithMessage("Invalid email format")
               .Must(_validationMethods.CheckIsEmailDoesntExists).WithMessage("Email is doesn't exist");


            RuleFor(account => account.Password)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8");
        }
    }
}
