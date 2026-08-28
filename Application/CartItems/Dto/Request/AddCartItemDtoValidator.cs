using FluentValidation;

namespace Application.CartItems.Dto.Request
{
    public class AddCartItemDtoValidator : AbstractValidator<AddCartItemDto>
    {
        public AddCartItemDtoValidator()
        {
            RuleFor(x => x.TicketTypeId)
                .NotEmpty().WithMessage("Ticket type is required.")
                .GreaterThan(0).WithMessage("Ticket type id must be greater than 0.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
