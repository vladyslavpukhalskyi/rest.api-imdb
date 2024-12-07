using Domain.Actors;

namespace Api.Dtos;

public record ActorDto(
    Guid? Id,
    string FirstName,
    string LastName,
    DateTime BirthDate)
{
    public static ActorDto FromDomainModel(Actor actor)
        => new(
            Id: actor.Id.Value,
            FirstName: actor.FirstName,
            LastName: actor.LastName,
            BirthDate: actor.BirthDate);
}