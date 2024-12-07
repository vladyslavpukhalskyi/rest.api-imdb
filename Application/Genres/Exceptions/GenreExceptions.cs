using Domain.Movies;

namespace Application.Genres.Exceptions;

public abstract class GenreException(GenreId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public GenreId GenreId { get; } = id;
}

public class GenreNotFoundException(GenreId id)
    : GenreException(id, $"Genre under id: {id} not found");

public class GenreAlreadyExistsException(GenreId id)
    : GenreException(id, $"Genre already exists: {id}");

public class GenreUnknownException(GenreId id, Exception innerException)
    : GenreException(id, $"Unknown exception for the genre under id: {id}", innerException);
