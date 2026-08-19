using FluentValidation;

namespace Application.DTOs.Category.Request
{
    public class CategoryDtoRequestValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryDtoRequestValidator() 
        {
            RuleFor(x => x.Name)
                            .NotEmpty().WithMessage("Name is required.")
                            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            // Description: optional, max 2000 chars
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
                .When(x => x.Description != null);
        }
    }
}
