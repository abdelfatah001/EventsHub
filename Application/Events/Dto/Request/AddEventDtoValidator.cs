using FluentValidation;

namespace Application.Events.Dto.Request
{
    public class AddEventDtoValidator : AbstractValidator<AddEventDto>
    {
        public AddEventDtoValidator()
        {
            // Title: required, 3..200 chars
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            // Description: optional, max 2000 chars
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
                .When(x => x.Description != null);

        }
    }
}
