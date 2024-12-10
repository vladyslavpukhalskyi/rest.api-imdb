namespace Domain.Movies;

public record MovieId(Guid Value)
{
    public static MovieId New() => new(Guid.NewGuid());
    public static MovieId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}