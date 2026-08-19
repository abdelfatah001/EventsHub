using FluentValidation;

namespace Application.DTOs.EventOccurences.Request
{
    public class AddOccurenceDtoValidator : AbstractValidator<AddOccurenceDto>
    {
        // Status: must be a valid enum value
        
        public AddOccurenceDtoValidator()
        {
            RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value.");

            // Start: required
            RuleFor(x => x.StartAt)
                .NotEmpty().WithMessage("Start date/time is required.");

            // End: required and must be after Start
            RuleFor(x => x.EndAt)
                .NotEmpty().WithMessage("End date/time is required.")
                .GreaterThan(x => x.StartAt).WithMessage("End date/time must be after Start date/time.");

            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.")
                .NotEmpty().WithMessage("Location is required");

            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("EventId is required.");
        }
    }
}
