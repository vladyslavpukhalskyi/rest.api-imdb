namespace Domain.Movies;

public record GenreId(Guid? Value)
{
    public static GenreId New() => new(Guid.NewGuid());
    public static GenreId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}