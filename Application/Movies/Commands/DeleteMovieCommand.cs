using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Movies.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Movies.Commands;

public record DeleteMovieCommand : IRequest<Result<Movie, MovieException>>
{
    public required Guid MovieId { get; init; }
}

public class DeleteMovieCommandHandler(
    IMovieRepository movieRepository)
    : IRequestHandler<DeleteMovieCommand, Result<Movie, MovieException>>
{
    public async Task<Result<Movie, MovieException>> Handle(
        DeleteMovieCommand request,
        CancellationToken cancellationToken)
    {
        var movieId = new MovieId(request.MovieId);

        var existingMovie = await movieRepository.GetById(movieId, cancellationToken);

        return await existingMovie.Match<Task<Result<Movie, MovieException>>>( 
            async m => await DeleteEntity(m, cancellationToken),
            () => Task.FromResult<Result<Movie, MovieException>>(new MovieNotFoundException(movieId)));
    }

    public async Task<Result<Movie, MovieException>> DeleteEntity(Movie movie, CancellationToken cancellationToken)
    {
        try
        {
            return await movieRepository.Delete(movie, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MovieUnknownException(movie.Id, exception);
        }
    }
}