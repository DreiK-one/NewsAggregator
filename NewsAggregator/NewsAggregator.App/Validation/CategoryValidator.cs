﻿using FluentValidation;
using NewsAggregator.App.Models;


namespace NewsAggregator.App.Validation
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name).NotNull()
                .MinimumLength(4).WithMessage("Minimum length of this field is 4")
                .MaximumLength(15).WithMessage("Maximum length of this field is 15")
                .Matches(@"^[A-Z][a-z]*$").WithMessage("You can use only letters and the first letter must be uppercase");
        }
    }
}
