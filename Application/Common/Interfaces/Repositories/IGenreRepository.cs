using Domain.Movies;
using Optional;

namespace Application.Common.Interfaces.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre> Add(Genre genre, CancellationToken cancellationToken);

        Task<Genre> Update(Genre genre, CancellationToken cancellationToken);

        Task<Genre> Delete(Genre genre, CancellationToken cancellationToken);

        Task<Option<Genre>> GetById(GenreId id, CancellationToken cancellationToken);

        Task<Option<Genre>> GetByName(string name, CancellationToken cancellationToken);
    }
}
