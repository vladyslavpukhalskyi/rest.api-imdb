using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Genres.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Genres.Commands;

public record UpdateGenreCommand : IRequest<Result<Genre, GenreException>>
{
    public required Guid? GenreId { get; init; }
    public required string Name { get; init; }
}

public class UpdateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<UpdateGenreCommand, Result<Genre, GenreException>>
{
    public async Task<Result<Genre, GenreException>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genreId = new GenreId(request.GenreId);

        var existingGenre = await genreRepository.GetById(genreId, cancellationToken);

        return await existingGenre.Match(
            async g => await UpdateEntity(g, request.Name, cancellationToken),
            () => Task.FromResult<Result<Genre, GenreException>>(new GenreNotFoundException(genreId)));
    }

    private async Task<Result<Genre, GenreException>> UpdateEntity(
        Genre entity,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateName(name);

            return await genreRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GenreUnknownException(entity.Id, exception);
        }
    }
}