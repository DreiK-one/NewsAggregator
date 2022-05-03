using FluentValidation;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.WebAPI.Models.Requests;


namespace NewsAggregator.WebAPI.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        private readonly IValidationMethodsCQS _validationMethods;
        public ChangePasswordValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(request => request.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8");


            RuleFor(request => request.NewPassword)
                .NotEmpty().WithMessage("New password is required")
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
                .Equal(request => request.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
