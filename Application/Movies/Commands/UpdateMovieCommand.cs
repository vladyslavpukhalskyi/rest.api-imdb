using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Movies.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Movies.Commands;

public record UpdateMovieCommand : IRequest<Result<Movie, MovieException>>
{
    public required Guid MovieId { get; init; }
    public required string Title { get; init; }
    public required string Genre { get; init; }
    public required DateTime ReleaseDate { get; init; }
}

public class UpdateMovieCommandHandler(IMovieRepository movieRepository) : IRequestHandler<UpdateMovieCommand, Result<Movie, MovieException>>
{
    public async Task<Result<Movie, MovieException>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movieId = new MovieId(request.MovieId);

        var existingMovie = await movieRepository.GetById(movieId, cancellationToken);

        return await existingMovie.Match(
            async m => await UpdateEntity(m, request.Title, request.Genre, request.ReleaseDate, cancellationToken),
            () => Task.FromResult<Result<Movie, MovieException>>(new MovieNotFoundException(movieId)));
    }

    private async Task<Result<Movie, MovieException>> UpdateEntity(
        Movie entity,
        string title,
        string genre, 
        DateTime releaseDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var genreId = new GenreId(Guid.Parse(genre)); 
            
            entity.UpdateDetails(title, releaseDate.Year, genreId);

            return await movieRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MovieUnknownException(entity.Id, exception);
        }
    }
}