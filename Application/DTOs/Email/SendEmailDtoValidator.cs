using FluentValidation;

namespace Application.DTOs.Email
{
    public class SendEmailDtoValidator : AbstractValidator<SendEmailDto>
    {
        public SendEmailDtoValidator()
        {
            RuleFor(x => x.To)
                .NotEmpty().WithMessage("Recipient email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.HtmlBody)
                .NotEmpty().WithMessage("Text body is required.");

            RuleFor(x => x.HtmlBody)
                .NotEmpty().WithMessage("Email body is required.");
        }
    }
}
