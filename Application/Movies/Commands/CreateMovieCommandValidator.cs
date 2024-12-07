using FluentValidation;

namespace Application.Movies.Commands;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.")
            .MinimumLength(1).WithMessage("Title must be at least 1 character long.");

        RuleFor(x => x.Director)
            .NotEmpty().WithMessage("Director is required.");

        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("Genre is required.");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release Date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Release Date cannot be in the future.");
    }
}