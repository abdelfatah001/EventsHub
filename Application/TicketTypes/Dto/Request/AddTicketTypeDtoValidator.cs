using FluentValidation;

namespace Application.TicketTypes.Dto.Request
{
    public class AddTicketTypeDtoValidator : AbstractValidator<AddTicketTypeDto>
    {
        public AddTicketTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                    .GreaterThan(0).WithMessage("Price must be positive integer");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                    .GreaterThan(0).WithMessage("Quantity must be positive integer");

            RuleFor(x => x.AvaliableQuantity)
                .NotEmpty().WithMessage("Avaliable Quantity is required.")
                    .GreaterThan(0).WithMessage("Avaliable Quantity must be positive integer")
                    .LessThanOrEqualTo(x => x.Quantity).WithMessage("Avaliable Quantity must be smaller than Quantity");

            RuleFor(x => x.OccurenceId)
                .NotEmpty().WithMessage("OccurenceId is required.");

        }
    }
}
