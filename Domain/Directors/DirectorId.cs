namespace Domain.Movies
{
    public record DirectorId(Guid Value)
    {
        public static DirectorId New() => new(Guid.NewGuid());
        public static DirectorId Empty() => new(Guid.Empty);
        public override string ToString() => Value.ToString();
    }
}