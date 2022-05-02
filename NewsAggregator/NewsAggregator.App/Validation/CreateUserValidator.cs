using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;


namespace NewsAggregator.App.Validation
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public CreateUserValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(user => user.Nickname)
                .NotNull().WithMessage("Nickname is required")
                .MinimumLength(4).WithMessage("Minimum length of nickname is 4")
                .Must(_validationMethods.CheckIsNicknameExists)
                    .WithMessage("This nickname is already exists");

            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(_validationMethods.CheckIsEmailExists)
                    .WithMessage("This email is already exists");

            RuleFor(user => user.Password)
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

            RuleFor(user => user.ConfirmPassword)
                .Equal(account => account.Password).WithMessage("Passwords do not match");
        }
    }
}
