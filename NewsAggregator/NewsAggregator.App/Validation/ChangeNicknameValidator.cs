using FluentValidation;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Validation
{
    public class ChangeNicknameValidator : AbstractValidator<ChangeNicknameViewModel>
    {
        private readonly IValidationMethods _validationMethods;
        public ChangeNicknameValidator(IValidationMethods validationMethods)
        {
            _validationMethods = validationMethods;

            RuleFor(model => model.CurrentNickname)
                .NotNull()
                .MinimumLength(4).WithMessage("Minimum length of nickname is 4")
                .Must(_validationMethods.CheckIsNicknameExists).WithMessage("User with this nickname is already exist")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Email can't contain word \"admin\"");

            RuleFor(model => model.NewNickname)
                .NotNull()
                .MinimumLength(4).WithMessage("Minimum length of password is 4")
                .Must(_validationMethods.CheckIsNicknameExists).WithMessage("User with this nickname is already exists")
                .Must(_validationMethods.CheckIsNicknameOrEmailContainsAdminWord)
                    .WithMessage("Nickname can't contain word \"admin\""); ;

            RuleFor(model => model.ConfirmNickname)
                .Equal(model => model.NewNickname).WithMessage("Nicknames do not match");
        }
    }
}
