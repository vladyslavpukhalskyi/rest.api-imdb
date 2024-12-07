using FluentValidation;

namespace Application.Genres.Commands;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");

        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("GenreId is required.");
    }
}