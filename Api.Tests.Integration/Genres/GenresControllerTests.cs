using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Movies;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Genres;

public class GenresControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Genre _mainGenre = GenresData.MainGenre;

    [Fact]
    public async Task ShouldCreateGenre()
    {
        // Arrange
        var genreName = "From Test Genre";
        var request = new GenreDto(
            Id: null,
            Name: genreName);

        // Act
        var response = await Client.PostAsJsonAsync("genres", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var genreFromResponse = await response.ToResponseModel<GenreDto>();
        var genreId = new GenreId(genreFromResponse.Id!.Value);

        var genreFromDatabase = await Context.Genres.FirstOrDefaultAsync(x => x.Id == genreId);
        genreFromDatabase.Should().NotBeNull();

        genreFromDatabase!.Name.Should().Be(genreName);
    }

    [Fact]
    public async Task ShouldUpdateGenre()
    {
        // Arrange
        var newGenreName = "New Genre Name";
        var request = new GenreDto(
            Id: _mainGenre.Id.Value,
            Name: newGenreName);

        // Act
        var response = await Client.PutAsJsonAsync("genres", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var genreFromResponse = await response.ToResponseModel<GenreDto>();

        var genreFromDatabase = await Context.Genres
            .FirstOrDefaultAsync(x => x.Id == new GenreId(genreFromResponse.Id!.Value));

        genreFromDatabase.Should().NotBeNull();

        genreFromDatabase!.Name.Should().Be(newGenreName);
    }

    [Fact]
    public async Task ShouldNotCreateGenreBecauseNameDuplicated()
    {
        // Arrange
        var request = new GenreDto(
            Id: null,
            Name: _mainGenre.Name);

        // Act
        var response = await Client.PostAsJsonAsync("genres", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateGenreBecauseGenreNotFound()
    {
        // Arrange
        var request = new GenreDto(
            Id: Guid.NewGuid(),
            Name: "New Genre Name");

        // Act
        var response = await Client.PutAsJsonAsync("genres", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task ShouldDeleteGenre()
    {
        // Arrange
        var genreToDeleteId = _mainGenre.Id.Value;

        // Act
        var response = await Client.DeleteAsync($"genres/{genreToDeleteId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var genreFromDatabase = await Context.Genres.FirstOrDefaultAsync(x => x.Id == new GenreId(genreToDeleteId));
        genreFromDatabase.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotDeleteGenreBecauseGenreNotFound()
    {
        // Arrange
        var nonExistentGenreId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"genres/{nonExistentGenreId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    public async Task InitializeAsync()
    {
        await Context.Genres.AddAsync(_mainGenre);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Genres.RemoveRange(Context.Genres);

        await SaveChangesAsync();
    }
}
