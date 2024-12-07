using FluentValidation;

namespace Application.Movies.Commands;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty();
    }
}