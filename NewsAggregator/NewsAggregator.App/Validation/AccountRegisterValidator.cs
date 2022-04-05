using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;
using System.Text.RegularExpressions;

namespace NewsAggregator.App.Validation
{
    public class AccountRegisterValidator : AbstractValidator<AccountRegisterModel>
    {
        private readonly IAccountService _accountService;
        public AccountRegisterValidator(IAccountService accountService)
        {
            _accountService = accountService;

            RuleFor(account => account.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(CheckIsEmailExists).WithMessage("This email is already exists");

            RuleFor(account => account.Password)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8")
                .Must(HasLowerCase)
                    .WithMessage("Password must contain at least one lowercase letter")
                .Must(HasUpperCase)
                    .WithMessage("Password must contain at least one uppercase letter")
                .Must(HasDigit)
                    .WithMessage("Password must contain at least one digit")
                .Must(HasSymbol)
                    .WithMessage("Password must contain at least one spacial symbol");

            RuleFor(account => account.ConfirmPassword)
                .Equal(account => account.Password).WithMessage("Passwords do not match");
            
        }

        private bool HasLowerCase(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            return lowercase.IsMatch(pw);
        }

        private bool HasUpperCase(string pw)
        {
            var uppercase = new Regex("[A-Z]+");
            return uppercase.IsMatch(pw);
        }

        private bool HasSymbol(string pw)
        {
            var symbol = new Regex("(\\W)+");
            return symbol.IsMatch(pw);
        }

        private bool HasDigit(string pw)
        {
            var digit = new Regex("(\\d)+");
            return digit.IsMatch(pw);
        }

        private bool CheckIsEmailExists(string email)
        {
            var result = _accountService.ValidateIsEmailExists(email);
            return !result;
        }
    }
}
