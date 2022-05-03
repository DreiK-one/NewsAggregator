using FluentValidation;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.WebAPI.Models.Requests;


namespace NewsAggregator.WebAPI.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        private readonly IValidationMethodsCQS _validationMethods;
        public RegisterValidator(IValidationMethodsCQS validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(_validationMethods.CheckIsEmailExists)
                    .WithMessage("This email is already exists")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Email can't contain word \"admin\"");

            RuleFor(request => request.Nickname)
                .NotEmpty().WithMessage("Nickname is required")
                .MinimumLength(4).WithMessage("Minimum length of nickname is 4")
                .Must(_validationMethods.CheckIsNicknameExists)
                    .WithMessage("This nickname is already exists")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Nickname can't contain word \"admin\""); ;

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8")
                .Must(_validationMethods.HasLowerCase)
                    .WithMessage("Password must contain at least one lowercase letter")
                .Must(_validationMethods.HasUpperCase)
                    .WithMessage("Password must contain at least one uppercase letter")
                .Must(_validationMethods.HasDigit)
                    .WithMessage("Password must contain at least one digit")
                .Must(_validationMethods.HasSymbol)
                    .WithMessage("Password must contain at least one spacial symbol");

            RuleFor(request => request.ConfirmPassword)
                .Equal(request => request.Password).WithMessage("Passwords do not match");
        }
    }
}
