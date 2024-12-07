using Domain.Directors;
using Domain.Movies;
using Optional;

namespace Application.Common.Interfaces.Queries
{
    public interface IDirectorQueries
    {
        Task<IReadOnlyList<Director>> GetAll(CancellationToken cancellationToken);

        Task<Option<Director>> GetById(DirectorId id, CancellationToken cancellationToken);

        Task<Option<Director>> GetByName(string name, CancellationToken cancellationToken);
    }
}