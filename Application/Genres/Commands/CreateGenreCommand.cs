using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Genres.Exceptions;
using Application.Movies.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Genres.Commands;

public record CreateGenreCommand : IRequest<Result<Genre, GenreException>>
{
    public required string Name { get; init; }
}

public class CreateGenreCommandHandler(
    IGenreRepository genreRepository)
    : IRequestHandler<CreateGenreCommand, Result<Genre, GenreException>>
{
    public async Task<Result<Genre, GenreException>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var existingGenre = await genreRepository.GetByName(request.Name, cancellationToken);

        return await existingGenre.Match<Task<Result<Genre, GenreException>>>(
            genre => Task.FromResult<Result<Genre, GenreException>>(new GenreAlreadyExistsException(genre.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<Genre, GenreException>> CreateEntity(
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            var genre = Genre.New(GenreId.New(), name);

            return await genreRepository.Add(genre, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GenreUnknownException(GenreId.Empty(), exception); 
        }
    }
}