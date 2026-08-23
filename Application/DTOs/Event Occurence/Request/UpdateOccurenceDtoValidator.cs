using FluentValidation;

namespace Application.DTOs.EventOccurences.Request
{
    public class UpdateOccurenceDtoValidator : AbstractValidator<UpdateOccurenceDto>
    {
        public UpdateOccurenceDtoValidator() 
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

            RuleFor(x => x.HallId)
                .NotEmpty().WithMessage("Hall Id is required");

        }
    }
}
