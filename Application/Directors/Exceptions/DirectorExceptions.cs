using Domain.Directors;
using Domain.Movies;

namespace Application.Directors.Exceptions;

public abstract class DirectorException(DirectorId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public DirectorId DirectorId { get; } = id;
}

public class DirectorNotFoundException(DirectorId id) 
    : DirectorException(id, $"Director under id: {id} not found");

public class DirectorAlreadyExistsException(DirectorId id) 
    : DirectorException(id, $"Director already exists: {id}");

public class DirectorUnknownException(DirectorId id, Exception innerException)
    : DirectorException(id, $"Unknown exception for the director under id: {id}", innerException);

public class DirectorNameNotValidException(string name)
    : DirectorException(DirectorId.Empty(), $"Invalid director name: '{name}'");