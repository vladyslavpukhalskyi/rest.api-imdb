using Domain.Movies;

namespace Api.Dtos;

public record GenreDto(
    Guid? Id,
    string Name)
{
    public static GenreDto FromDomainModel(Genre genre)
        => new(
            Id: genre.Id.Value,
            Name: genre.Name);
}