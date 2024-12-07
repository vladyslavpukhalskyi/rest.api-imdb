using Domain.Actors;
using Domain.Movies;

namespace Application.Movies.Exceptions;

public abstract class MovieException(MovieId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public MovieId MovieId { get; } = id;
}

public class MovieNotFoundException(MovieId id) : MovieException(id, $"Movie under id: {id} not found");

public class MovieAlreadyExistsException(MovieId id) : MovieException(id, $"Movie already exists: {id}");

public class MovieGenreNotFoundException(GenreId genre) : MovieException(MovieId.Empty(), $"Genre '{genre}' not found");

public class MovieUnknownException(MovieId id, Exception innerException)
    : MovieException(id, $"Unknown exception for the movie under id: {id}", innerException);
    
public class MovieDirectorNotFoundException(DirectorId director) : MovieException(MovieId.Empty(), $"Director '{director}' not found");

public class MovieActorNotFoundException(ActorId actor) : MovieException(MovieId.Empty(), $"Actor '{actor}' not found");