using FluentValidation;


namespace Application.Halls.Dto.Request
{
    public class AddHallDtoValidator : AbstractValidator<AddHallDto>
    {
        public AddHallDtoValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Title is required.")
                    .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                    .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Address)
                    .NotEmpty().WithMessage("Address is required.")
                    .MaximumLength(2000).WithMessage("Address must not exceed 2000 characters.");

            RuleFor(x => x.Capacity)
                    .NotEmpty().WithMessage("Capacity is required.")
                    .GreaterThan(0).WithMessage("Capacity must be positive integer");
        }
    }
}
