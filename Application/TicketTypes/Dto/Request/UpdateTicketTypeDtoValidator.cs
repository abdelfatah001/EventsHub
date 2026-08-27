using FluentValidation;

namespace Application.TicketTypes.Dto.Request
{
    public class UpdateTicketTypeDtoValidator : AbstractValidator<UpdateTicketTypeDto>
    {
        public UpdateTicketTypeDtoValidator()
        {
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

        }
    }
}
