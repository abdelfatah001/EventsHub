using FluentValidation;


namespace Application.Users.Dto.Request
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Confirm password must match the new password.");
        }
    }
}
