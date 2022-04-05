using FluentValidation;
using NewsAggregator.App.Models;

namespace NewsAggregator.App.Validation
{
    public class AccountLoginValidator : AbstractValidator<AccountLoginModel>
    {
        public AccountLoginValidator()
        {
            RuleFor(account => account.Email)
               .NotNull().WithMessage("Email is required");


            RuleFor(account => account.Password)
                .NotNull().WithMessage("Password is required");
                //.Must(HasValidPassword)
                //.WithMessage("Incorrect password");
        }
    }
}
