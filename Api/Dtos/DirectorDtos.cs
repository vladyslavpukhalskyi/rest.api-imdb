using Domain.Directors;

namespace Api.Dtos;

public record DirectorDto(
    Guid? Id,
    string FirstName,
    string LastName,
    DateTime BirthDate)
{
    public static DirectorDto FromDomainModel(Director director)
        => new(
            Id: director.Id.Value,
            FirstName: director.FirstName,
            LastName: director.LastName,
            BirthDate: director.BirthDate);
}