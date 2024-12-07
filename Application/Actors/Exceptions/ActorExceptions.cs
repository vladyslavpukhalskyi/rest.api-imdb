using Domain.Actors;

namespace Application.Actors.Exceptions
{
    public abstract class ActorException : Exception
    {
        public ActorId ActorId { get; }

        protected ActorException(ActorId id, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            ActorId = id;
        }
    }

    public class ActorNotFoundException : ActorException
    {
        public ActorNotFoundException(ActorId id)
            : base(id, $"Actor with id: {id} not found") { }
    }

    public class ActorAlreadyExistsException : ActorException
    {
        public ActorAlreadyExistsException(ActorId id)
            : base(id, $"Actor already exists: {id}") { }
    }

    public class ActorUnknownException : ActorException
    {
        public ActorUnknownException(ActorId id, Exception innerException)
            : base(id, $"Unknown exception for actor with id: {id}", innerException) { }
    }
}