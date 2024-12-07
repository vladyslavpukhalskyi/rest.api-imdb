namespace Domain.Actors
{
    public record ActorId(Guid Value)
    {
        public static ActorId New() => new(Guid.NewGuid());
        public static ActorId Empty() => new(Guid.Empty);
        public override string ToString() => Value.ToString();
    }
}