using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Actors.Exceptions;
using Domain.Actors;
using MediatR;

namespace Application.Actors.Commands
{
    public record UpdateActorCommand : IRequest<Result<Actor, ActorException>>
    {
        public required Guid ActorId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required DateTime BirthDate { get; init; }
    }

    public class UpdateActorCommandHandler(
        IActorRepository actorRepository)
        : IRequestHandler<UpdateActorCommand, Result<Actor, ActorException>>
    {
        public async Task<Result<Actor, ActorException>> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actorId = new ActorId(request.ActorId);

            var existingActor = await actorRepository.GetById(actorId, cancellationToken);

            return await existingActor.Match<Task<Result<Actor, ActorException>>>(
                async a => await UpdateEntity(a, request.FirstName, request.LastName, request.BirthDate, cancellationToken),
                () => Task.FromResult<Result<Actor, ActorException>>(new ActorNotFoundException(actorId)));
        }

        private async Task<Result<Actor, ActorException>> UpdateEntity(
            Actor actor,
            string firstName,
            string lastName,
            DateTime birthDate,
            CancellationToken cancellationToken)
        {
            try
            {
                actor.UpdateDetails(firstName, lastName, birthDate);

                return await actorRepository.Update(actor, cancellationToken);
            }
            catch (Exception exception)
            {
                return new ActorUnknownException(actor.Id, exception);
            }
        }
    }
}