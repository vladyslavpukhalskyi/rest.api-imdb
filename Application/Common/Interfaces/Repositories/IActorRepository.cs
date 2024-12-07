using Domain.Actors;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IActorRepository
{
    Task<Actor> Add(Actor actor, CancellationToken cancellationToken);
    Task<Actor> Update(Actor actor, CancellationToken cancellationToken);
    Task<Actor> Delete(Actor actor, CancellationToken cancellationToken);
    Task<Option<Actor>> GetById(ActorId id, CancellationToken cancellationToken);
    Task<Option<Actor>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken);
}