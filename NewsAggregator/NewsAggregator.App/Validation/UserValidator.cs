using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Validation
{
    public class UserValidator : AbstractValidator<CreateOrEditUserViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public UserValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(user => user.Nickname)
               .NotNull().WithMessage("Nickname is required")
               .MinimumLength(4).WithMessage("Minimum length of nickname is 4")
               .Must(_validationMethods.CheckIsNicknameExists).WithMessage("User with this nickname is already exist");


            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(_validationMethods.CheckIsEmailExists).WithMessage("User with this email is already exists");
        }
    }
}
