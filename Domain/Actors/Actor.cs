using System;
using System.Collections.Generic;
using Domain.Movies;

namespace Domain.Actors
{
    public class Actor
    {
        public ActorId Id { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime BirthDate { get; private set; }

        public List<Movie> Movies { get; private set; } = new List<Movie>();
        
        public string Name => $"{FirstName} {LastName}";

        private Actor(ActorId id, string firstName, string lastName, DateTime birthDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public static Actor New(ActorId id, string firstName, string lastName, DateTime birthDate)
            => new(id, firstName, lastName, birthDate);

        public void UpdateDetails(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
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