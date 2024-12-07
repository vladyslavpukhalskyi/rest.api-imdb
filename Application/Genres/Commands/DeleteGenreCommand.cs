using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Genres.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Genres.Commands
{
    public record DeleteGenreCommand : IRequest<Result<Genre, GenreException>>
    {
        public required Guid GenreId { get; init; }
    }

    public class DeleteGenreCommandHandler(
        IGenreRepository genreRepository)
        : IRequestHandler<DeleteGenreCommand, Result<Genre, GenreException>>
    {
        public async Task<Result<Genre, GenreException>> Handle(
            DeleteGenreCommand request,
            CancellationToken cancellationToken)
        {
            var genreId = new GenreId(request.GenreId);

            var existingGenre = await genreRepository.GetById(genreId, cancellationToken);

            return await existingGenre.Match<Task<Result<Genre, GenreException>>>(
                async g => await DeleteEntity(g, cancellationToken),
                () => Task.FromResult<Result<Genre, GenreException>>(new GenreNotFoundException(genreId)));
        }

        public async Task<Result<Genre, GenreException>> DeleteEntity(Genre genre, CancellationToken cancellationToken)
        {
            try
            {
                return await genreRepository.Delete(genre, cancellationToken);
            }
            catch (Exception exception)
            {
                return new GenreUnknownException(genre.Id, exception);
            }
        }
    }
}