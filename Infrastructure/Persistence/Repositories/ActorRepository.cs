using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Actors;
using Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ActorRepository : IActorRepository, IActorQueries
{
    private readonly ApplicationDbContext _context;

    public ActorRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Option<Actor>> GetById(ActorId id, CancellationToken cancellationToken)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var entity = await _context.Actors

            .Include(a => a.Movies) // Завантаження пов'язаних фільмів
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Actor>() : Option.Some(entity);
    }

    public async Task<Option<Actor>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        var entity = await _context.Actors
            .AsNoTracking()
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName, cancellationToken);

        return entity == null ? Option.None<Actor>() : Option.Some(entity);
    }

    public async Task<Option<Actor>> GetByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        var entity = await _context.Actors
            .AsNoTracking()
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(a => $"{a.FirstName} {a.LastName}" == name, cancellationToken);

        return entity == null ? Option.None<Actor>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Actor>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Actors
            .AsNoTracking()
            .Include(a => a.Movies)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Actor>> GetByMovieId(MovieId movieId, CancellationToken cancellationToken)
    {
        if (movieId == null)
            throw new ArgumentNullException(nameof(movieId));

        return await _context.Actors
            .Where(a => a.Movies.Any(m => m.Id == movieId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Actor> Add(Actor actor, CancellationToken cancellationToken)
    {
        if (actor == null)
            throw new ArgumentNullException(nameof(actor));

        await _context.Actors.AddAsync(actor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return actor;
    }

    public async Task<Actor> Update(Actor actor, CancellationToken cancellationToken)
    {
        if (actor == null)
            throw new ArgumentNullException(nameof(actor));

        _context.Actors.Update(actor);
        await _context.SaveChangesAsync(cancellationToken);

        return actor;
    }

    public async Task<Actor> Delete(Actor actor, CancellationToken cancellationToken)
    {
        if (actor == null)
            throw new ArgumentNullException(nameof(actor));

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync(cancellationToken);

        return actor;
    }
}
