using FluentValidation;


namespace Application.DTOs.Hall.Request
{
    public class UpdateHallDtoValidator : AbstractValidator<UpdateHallDto>
    {
        public UpdateHallDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity is required.")
                .GreaterThan(0).WithMessage("Capacity must be positive integer");


        }
    }
}
