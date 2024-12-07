using Domain.Movies;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IMovieRepository
{
    Task<Movie> Add(Movie movie, CancellationToken cancellationToken);
    Task<Movie> Update(Movie movie, CancellationToken cancellationToken);
    Task<Movie> Delete(Movie movie, CancellationToken cancellationToken);
    Task<Option<Movie>> GetById(MovieId id, CancellationToken cancellationToken);
    
    Task<Option<Movie>> GetByTitle(string title, CancellationToken cancellationToken);
}
