using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Directors;
using Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class DirectorRepository(ApplicationDbContext context) : IDirectorRepository, IDirectorQueries
{
    public async Task<Option<Director>> GetById(DirectorId id, CancellationToken cancellationToken)
    {
        var entity = await context.Directors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Director>() : Option.Some(entity);
    }

    Task<Option<Director>> IDirectorRepository.GetById(DirectorId id, CancellationToken cancellationToken)
    {
        return GetById(id, cancellationToken);
    }

    Task<Option<Director>> IDirectorQueries.GetById(DirectorId id, CancellationToken cancellationToken)
    {
        return GetById(id, cancellationToken);
    }

    public async Task<Option<Director>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Directors
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => (x.FirstName + " " + x.LastName) == name, 
                cancellationToken
            );

        return entity == null ? Option.None<Director>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Director>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Directors
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Director> Add(Director director, CancellationToken cancellationToken)
    {
        await context.Directors.AddAsync(director, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return director;
    }

    public async Task<Director> Update(Director director, CancellationToken cancellationToken)
    {
        context.Directors.Update(director);

        await context.SaveChangesAsync(cancellationToken);

        return director;
    }

    public async Task<Director> Delete(Director director, CancellationToken cancellationToken)
    {
        context.Directors.Remove(director);

        await context.SaveChangesAsync(cancellationToken);

        return director;
    }
}