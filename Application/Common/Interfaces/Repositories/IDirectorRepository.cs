using Domain.Directors;
using Domain.Movies;
using Optional;

namespace Application.Common.Interfaces.Repositories
{
    public interface IDirectorRepository
    {
        Task<Director> Add(Director director, CancellationToken cancellationToken);

        Task<Director> Update(Director director, CancellationToken cancellationToken);

        Task<Director> Delete(Director director, CancellationToken cancellationToken);

        Task<Option<Director>> GetById(DirectorId id, CancellationToken cancellationToken);

        Task<Option<Director>> GetByName(string name, CancellationToken cancellationToken);
    }
}