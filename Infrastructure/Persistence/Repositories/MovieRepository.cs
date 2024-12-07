using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class MovieRepository(ApplicationDbContext context) : IMovieRepository, IMovieQueries
{
    public async Task<Option<Movie>> GetById(MovieId id, CancellationToken cancellationToken)
    {
        var entity = await context.Movies
            .Include(x => x.Genre) 
            .Include(x => x.Director)
            .Include(x => x.Actors)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Movie>() : Option.Some(entity);
    }

    public Task<Option<Movie>> GetByTitleAndReleaseYear(string title, int releaseYear, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Movie>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Movies
            .Include(x => x.Genre)  
            .Include(x => x.Director)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

  

    public async Task<Option<Movie>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity == null ? Option.None<Movie>() : Option.Some(entity);
    }

    public async Task<Movie> Add(Movie movie, CancellationToken cancellationToken)
    {
        await context.Movies.AddAsync(movie, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return movie;
    }

    public async Task<Movie> Update(Movie movie, CancellationToken cancellationToken)
    {
        context.Movies.Update(movie);

        await context.SaveChangesAsync(cancellationToken);

        return movie;
    }

    public async Task<Movie> Delete(Movie movie, CancellationToken cancellationToken)
    {
        context.Movies.Remove(movie);

        await context.SaveChangesAsync(cancellationToken);

        return movie;
    }
}
