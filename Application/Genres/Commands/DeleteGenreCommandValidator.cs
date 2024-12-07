using FluentValidation;

namespace Application.Genres.Commands;

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(x => x.GenreId).NotEmpty().WithMessage("Genre ID is required.");
    }
}