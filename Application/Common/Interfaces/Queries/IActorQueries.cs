using Domain.Actors;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IActorQueries
{
    Task<IReadOnlyList<Actor>> GetAll(CancellationToken cancellationToken);
    Task<Option<Actor>> GetById(ActorId id, CancellationToken cancellationToken);
}