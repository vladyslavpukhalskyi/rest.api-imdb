using FluentValidation;

namespace Application.Genres.Commands;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
            .MinimumLength(1).WithMessage("Name must be at least 1 character long.");
    }
}