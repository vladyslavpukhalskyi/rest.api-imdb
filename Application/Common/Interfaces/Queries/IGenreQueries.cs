using Domain.Movies;
using Optional;

namespace Application.Common.Interfaces.Queries
{
    public interface IGenreQueries
    {
        Task<IReadOnlyList<Genre>> GetAll(CancellationToken cancellationToken);

        Task<Option<Genre>> GetById(GenreId id, CancellationToken cancellationToken);

        Task<Option<Genre>> GetByName(string name, CancellationToken cancellationToken);
    }
}