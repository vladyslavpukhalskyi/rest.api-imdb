using FluentValidation;

namespace Application.Movies.Commands;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255).MinimumLength(2);
        RuleFor(x => x.Genre).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.ReleaseDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);  
    }
}