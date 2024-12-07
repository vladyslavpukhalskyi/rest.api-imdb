using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Actors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Actors;

public class ActorsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Actor _mainActor;

    public ActorsControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
        _mainActor = Actor.New(ActorId.New(), "Test", "Actor", new DateTime(1980, 1, 1));
    }

    public async Task InitializeAsync()
    {
        await Context.Actors.AddAsync(_mainActor);
        await Context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Actors.RemoveRange(Context.Actors);
        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldCreateActor()
    {
        // Arrange
        var request = new ActorDto(
            Id: null,
            FirstName: "New",
            LastName: "Actor",
            BirthDate: new DateTime(1990, 1, 1));

        // Act
        var response = await Client.PostAsJsonAsync("actors", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var actorFromResponse = await response.Content.ReadFromJsonAsync<ActorDto>();
        var actorId = new ActorId(actorFromResponse!.Id!.Value);

        var actorFromDatabase = await Context.Actors.FirstOrDefaultAsync(x => x.Id == actorId);
        actorFromDatabase.Should().NotBeNull();
        actorFromDatabase!.FirstName.Should().Be("New");
        actorFromDatabase.LastName.Should().Be("Actor");
        actorFromDatabase.BirthDate.Should().Be(new DateTime(1990, 1, 1));
    }

    [Fact]
    public async Task ShouldUpdateActor()
    {
        // Arrange
        var newFirstName = "Updated";
        var newLastName = "Name";
        var newBirthDate = new DateTime(1985, 5, 5);

        var request = new ActorDto(
            Id: _mainActor.Id.Value,
            FirstName: newFirstName,
            LastName: newLastName,
            BirthDate: newBirthDate);

        // Act
        var response = await Client.PutAsJsonAsync("actors", request);

        // Assert 
        response.IsSuccessStatusCode.Should().BeTrue();

        // Fetch 
        var updatedActorFromDatabase = await Context.Actors.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == _mainActor.Id);
    
        // Ensure 
        updatedActorFromDatabase.Should().NotBeNull();
        updatedActorFromDatabase!.FirstName.Should().Be(newFirstName);
        updatedActorFromDatabase.LastName.Should().Be(newLastName);
        updatedActorFromDatabase.BirthDate.Should().Be(newBirthDate);
    }
    
    [Fact]
    public async Task ShouldDeleteActor()
    {
        // Act
        var response = await Client.DeleteAsync($"actors/{_mainActor.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var actorFromDatabase = await Context.Actors.FirstOrDefaultAsync(x => x.Id == _mainActor.Id);
        actorFromDatabase.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnConflictWhenCreatingActorThatAlreadyExists()
    {
        // Arrange
        var request = new ActorDto(
            Id: null,
            FirstName: _mainActor.FirstName,
            LastName: _mainActor.LastName,
            BirthDate: _mainActor.BirthDate);

        // Act
        var existingActor = await Context.Actors
            .FirstOrDefaultAsync(a => a.FirstName == request.FirstName && a.LastName == request.LastName && a.BirthDate == request.BirthDate);
        
        if (existingActor != null)
        {
            var response = await Client.PostAsJsonAsync("actors", request);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            
            var actorsInDatabase = await Context.Actors.ToListAsync();
            actorsInDatabase.Count.Should().Be(1);  
        }
        else
        {

            var response = await Client.PostAsJsonAsync("actors", request);
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentActor()
    {
        // Arrange
        var nonExistentActorId = Guid.NewGuid(); // Random GUID
        var request = new ActorDto(
            Id: nonExistentActorId,
            FirstName: "Updated",
            LastName: "Name",
            BirthDate: new DateTime(1985, 5, 5));

        // Act
        var response = await Client.PutAsJsonAsync("actors", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentActor()
    {
        // Arrange
        var nonExistentActorId = Guid.NewGuid(); 

        // Act
        var response = await Client.DeleteAsync($"actors/{nonExistentActorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var actorsInDatabase = await Context.Actors.ToListAsync();
        actorsInDatabase.Count.Should().Be(1); 
    }
}
