using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Actors.Exceptions;
using Domain.Actors;
using MediatR;

namespace Application.Actors.Commands
{
    public record DeleteActorCommand : IRequest<Result<Actor, ActorException>>
    {
        public required Guid ActorId { get; init; }
    }

    public class DeleteActorCommandHandler(
        IActorRepository actorRepository)
        : IRequestHandler<DeleteActorCommand, Result<Actor, ActorException>>
    {
        public async Task<Result<Actor, ActorException>> Handle(
            DeleteActorCommand request,
            CancellationToken cancellationToken)
        {
            var actorId = new ActorId(request.ActorId);

            var existingActor = await actorRepository.GetById(actorId, cancellationToken);

            return await existingActor.Match<Task<Result<Actor, ActorException>>>(
                async a => await DeleteEntity(a, cancellationToken),
                () => Task.FromResult<Result<Actor, ActorException>>(new ActorNotFoundException(actorId)));
        }

        public async Task<Result<Actor, ActorException>> DeleteEntity(Actor actor, CancellationToken cancellationToken)
        {
            try
            {
                return await actorRepository.Delete(actor, cancellationToken);
            }
            catch (Exception exception)
            {
                return new ActorUnknownException(actor.Id, exception);
            }
        }
    }
}