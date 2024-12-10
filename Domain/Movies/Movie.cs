using Domain.Actors;
using Domain.Directors;
using Domain.Movies;

public class Movie
{
    public MovieId Id { get; }
    public string Title { get; private set; }
    public int ReleaseYear { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public GenreId GenreId { get; private set; } 
    public Genre? Genre { get; } 

    public DirectorId DirectorId { get; }
    public Director? Director { get; }

    public List<Actor> Actors { get; private set; } = new List<Actor>();

    private Movie(MovieId id, string title, int releaseYear, DateTime updatedAt, GenreId genreId, DirectorId directorId)
    {
        Id = id;
        Title = title;
        ReleaseYear = releaseYear;
        UpdatedAt = updatedAt;
        GenreId = genreId;
        DirectorId = directorId;
    }

    public static Movie New(MovieId id, string title, int releaseYear, GenreId genreId, DirectorId directorId)
        => new(id, title, releaseYear, DateTime.UtcNow, genreId, directorId);

    public void UpdateDetails(string title, int releaseYear, GenreId genreId)
    {
        Title = title;
        ReleaseYear = releaseYear;
        GenreId = genreId; 
        UpdatedAt = DateTime.UtcNow;
    }


    public void AddActor(Actor actor)
    {
        if (!Actors.Contains(actor))
        {
            Actors.Add(actor);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveActor(Actor actor)
    {
        if (Actors.Contains(actor))
        {
            Actors.Remove(actor);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}