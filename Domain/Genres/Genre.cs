using System;
using System.Collections.Generic;

namespace Domain.Movies
{
    public class Genre
    {
        public GenreId Id { get; }
        public string Name { get; private set; }

        public List<Movie> Movies { get; private set; } = new List<Movie>();

        private Genre(GenreId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Genre New(GenreId id, string name)
            => new(id, name);

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void AddMovie(Movie movie)
        {
            if (!Movies.Contains(movie))
            {
                Movies.Add(movie);
            }
        }

        public void RemoveMovie(Movie movie)
        {
            if (Movies.Contains(movie))
            {
                Movies.Remove(movie);
            }
        }
    }
}