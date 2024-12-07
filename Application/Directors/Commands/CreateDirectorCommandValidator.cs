using FluentValidation;

namespace Application.Directors.Commands;

public class CreateDirectorCommandValidator : AbstractValidator<CreateDirectorCommand>
{
    public CreateDirectorCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(255).WithMessage("First Name must not exceed 255 characters.")
            .MinimumLength(1).WithMessage("First Name must be at least 1 character long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(255).WithMessage("Last Name must not exceed 255 characters.")
            .MinimumLength(1).WithMessage("Last Name must be at least 1 character long.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth Date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birth Date cannot be in the future.");
    }
}