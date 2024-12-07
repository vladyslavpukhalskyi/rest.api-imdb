using System;
using System.Collections.Generic;
using Domain.Actors;
using Domain.Directors;
using Domain.Movies;

namespace Tests.Data
{
    public static class MoviesData
    {
        public static Movie MainMovie => Movie.New(
            MovieId.New(),
            "Main Test Movie",
            2024,
            GenresData.MainGenre.Id,
            DirectorsData.MainDirector.Id
        ).WithActors(new List<Actor>
        {
            ActorsData.MainActor,
            ActorsData.SecondaryActor
        });

        public static Movie SecondaryMovie => Movie.New(
            MovieId.New(),
            "Secondary Test Movie",
            2023,
            GenresData.MainGenre.Id, 
            DirectorsData.SecondaryDirector.Id
        ).WithActors(new List<Actor>
        {
            ActorsData.SecondaryActor
        });
        
        private static Movie WithActors(this Movie movie, List<Actor> actors)
        {
            foreach (var actor in actors)
            {
                movie.AddActor(actor);
            }
            return movie;
        }
    }
}