using FluentValidation;

namespace Application.Users.Dto.Request
{
    public class UpdateEmailDtoValidator : AbstractValidator<UpdateEmailDto>
    {
        public UpdateEmailDtoValidator() 
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("New email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }

    }
}
