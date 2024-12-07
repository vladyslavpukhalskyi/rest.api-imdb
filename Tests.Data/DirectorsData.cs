using Domain.Directors;
using System;
using Domain.Movies;

namespace Tests.Data
{
    public static class DirectorsData
    {
        public static Director MainDirector => Director.New(
            DirectorId.New(),
            "TestFirstName",
            "TestLastName",
            new DateTime(1975, 5, 15));
        
        public static Director SecondaryDirector => Director.New(
            DirectorId.New(),
            "SecondaryFirstName",
            "SecondaryLastName",
            new DateTime(1980, 12, 25));
    }
}