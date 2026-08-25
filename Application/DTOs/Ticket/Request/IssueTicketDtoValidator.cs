using FluentValidation;

namespace Application.DTOs.Ticket.Request
{
    public class IssueTicketDtoValidator : AbstractValidator<IssueTicketDto>
    {
        public IssueTicketDtoValidator() 
        {

            RuleFor(x => x.TicketTypeId)
                .NotEmpty().WithMessage("Ticket Type Id is required.")
                .GreaterThan(0).WithMessage("Ticket Type Id must be positive integer");

            RuleFor(x => x.OrderItemId)
                .NotEmpty().WithMessage("Order Item Id is required.")
                .GreaterThan(0).WithMessage("Order Item Id must be positive integer");
        }
    }
}
