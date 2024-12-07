using Api.Dtos;

public record MovieDto(
    Guid? Id,
    string Title,
    int ReleaseYear,
    Guid? GenreId,
    GenreDto? Genre,
    Guid DirectorId,
    DirectorDto? Director,
    List<Guid> ActorIds, 
    List<ActorDto>? Actors)
{
    public static MovieDto FromDomainModel(Movie movie)
        => new(
            Id: movie.Id.Value,
            Title: movie.Title,
            ReleaseYear: movie.ReleaseYear,
            GenreId: movie.GenreId.Value,
            Genre: movie.Genre == null ? null : GenreDto.FromDomainModel(movie.Genre),
            DirectorId: movie.DirectorId.Value,
            Director: movie.Director == null ? null : DirectorDto.FromDomainModel(movie.Director),
            ActorIds: movie.Actors.Select(a => a.Id.Value).ToList(),
            Actors: movie.Actors?.Select(ActorDto.FromDomainModel).ToList());
}