using FluentValidation;

namespace Application.Directors.Commands;

public class DeleteDirectorCommandValidator : AbstractValidator<DeleteDirectorCommand>
{
    public DeleteDirectorCommandValidator()
    {
        RuleFor(x => x.DirectorId)
            .NotEmpty().WithMessage("Director Id is required.");
    }
}