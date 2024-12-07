using Domain.Movies;

namespace Tests.Data;

public static class GenresData
{
    public static Genre MainGenre => Genre.New(GenreId.New(), "Main Test Genre");
}