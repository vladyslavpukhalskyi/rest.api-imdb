using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Directors;
using Domain.Movies;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Movies
{
    public class MoviesControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Director _mainDirector;
        private readonly Genre _mainGenre;

        public MoviesControllerTests(IntegrationTestWebFactory factory)
            : base(factory)
        {
            _mainDirector = Director.New(DirectorId.New(), "Test", "Director", new DateTime(1975, 5, 15));
            _mainGenre = Genre.New(GenreId.New(), "Action");
        }

        public async Task InitializeAsync()
        {
            await Context.Directors.AddAsync(_mainDirector);
            await Context.Genres.AddAsync(_mainGenre);
            await Context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Directors.RemoveRange(Context.Directors);
            Context.Genres.RemoveRange(Context.Genres);
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldCreateMovie()
        {
            // Arrange
            var request = new MovieDto(
                Id: null,
                Title: "Test Movie",
                ReleaseYear: 2024,
                GenreId: _mainGenre.Id.Value,
                Genre: null,  
                DirectorId: _mainDirector.Id.Value,
                Director: null, 
                ActorIds: new List<Guid>(),
                Actors: null 
            );

            // Act
            var response = await Client.PostAsJsonAsync("movies", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            
            var movieFromResponse = await response.Content.ReadFromJsonAsync<MovieDto>();
            var movieId = new MovieId(movieFromResponse!.Id!.Value);
            
            var movieFromDatabase = await Context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            movieFromDatabase.Should().NotBeNull();
            movieFromDatabase!.Title.Should().Be("Test Movie");
            movieFromDatabase.ReleaseYear.Should().Be(2024);
            movieFromDatabase.DirectorId.Should().Be(_mainDirector.Id);
            movieFromDatabase.GenreId.Should().Be(_mainGenre.Id);
        }

    }
}
