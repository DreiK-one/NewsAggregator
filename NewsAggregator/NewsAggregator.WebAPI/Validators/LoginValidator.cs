using FluentValidation;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.WebAPI.Models.Requests;


namespace NewsAggregator.WebAPI.Validators
{
    public class LoginValidator : AbstractValidator<AuthenticateRequest>
    {
        private readonly IValidationMethodsCQS _validationMethods;
        public LoginValidator()
        {
            RuleFor(request => request.Login)
               .NotNull().WithMessage("Login is required")
               .EmailAddress().WithMessage("Invalid email format")
               .Must(_validationMethods.CheckIsEmailDoesntExists).WithMessage("Email is doesn't exist");


            RuleFor(request => request.Password)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Minimum length of password is 8");
        }
    }
}
