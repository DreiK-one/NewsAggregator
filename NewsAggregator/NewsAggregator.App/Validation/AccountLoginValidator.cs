using FluentValidation;
using NewsAggregator.App.Models;

namespace NewsAggregator.App.Validation
{
    public class AccountLoginValidator : AbstractValidator<AccountLoginModel>
    {
        public AccountLoginValidator()
        {
            RuleFor(account => account.Email)
               .NotNull().WithMessage("Email is required")
               .EmailAddress().WithMessage("Invalid email format");


            RuleFor(account => account.Password)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8");
        }
    }
}
