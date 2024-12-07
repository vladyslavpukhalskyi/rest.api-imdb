using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Movies.Exceptions;
using Domain.Actors;
using Domain.Movies;
using MediatR;

namespace Application.Movies.Commands;

public record CreateMovieCommand : IRequest<Result<Movie, MovieException>>
{
    public required string Title { get; init; }
    public required Guid Director { get; init; }
    public required Guid? GenreId { get; init; }
    public required IReadOnlyList<Guid> ActorIds { get; init; }
    public required DateTime ReleaseDate { get; init; }
}

public class CreateMovieCommandHandler(
    IMovieRepository movieRepository,
    IGenreRepository genreRepository, IActorQueries actorQueries, IDirectorQueries directorQueries)
    : IRequestHandler<CreateMovieCommand, Result<Movie, MovieException>>
{
    public async Task<Result<Movie, MovieException>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var directorId = new DirectorId(request.Director);
        var existingDirector = await directorQueries.GetById(directorId, cancellationToken);

        return await existingDirector.Match(
            async d =>
            {
                var genreId = new GenreId(request.GenreId);
                
                var genre = await genreRepository.GetById(genreId, cancellationToken);

                return await genre.Match<Task<Result<Movie, MovieException>>>( 
                    async g =>
                    {
                        var existingMovie = await movieRepository.GetByTitle(request.Title, cancellationToken);

                        return await existingMovie.Match(
                            m => Task.FromResult<Result<Movie, MovieException>>(new MovieAlreadyExistsException(m.Id)),
                            async () => await CreateEntity(request.Title, directorId, g.Id, request.ReleaseDate, request.ActorIds, cancellationToken));
                    },
                    () => Task.FromResult<Result<Movie, MovieException>>(new MovieGenreNotFoundException(genreId)));
            },
            () => Task.FromResult<Result<Movie, MovieException>>(new MovieDirectorNotFoundException(directorId)));
        
    }

    private async Task<Result<Movie, MovieException>> CreateEntity(
        string title,
        DirectorId directorId,
        GenreId genreId,
        DateTime releaseDate,
        IReadOnlyList<Guid> actorIds,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Movie.New(MovieId.New(), title, releaseDate.Year, genreId, directorId);
             await movieRepository.Add(entity, cancellationToken);
            foreach (var actorId in actorIds)
            {
                var id = new ActorId(actorId);
                var actorResult = await actorQueries.GetById(id, cancellationToken);
                
                var actor = await actorResult.Match(
                    async a =>
                    {
                        entity.AddActor(a);
                        return a;
                    },
                    () => Task.FromResult<Actor>(null));
                
                if (actor == null)
                {
                    return new MovieActorNotFoundException(id);
                }
            }
            
            return await movieRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MovieUnknownException(MovieId.Empty(), exception);
        }
    }

}
