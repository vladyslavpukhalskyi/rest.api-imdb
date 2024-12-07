using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Directors;
using Domain.Movies;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Directors
{
    public class DirectorsControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Director _mainDirector;

        public DirectorsControllerTests(IntegrationTestWebFactory factory)
            : base(factory)
        {
            _mainDirector = Director.New(DirectorId.New(), "Test", "Director", new DateTime(1975, 5, 15));
        }

        public async Task InitializeAsync()
        {
            await Context.Directors.AddAsync(_mainDirector);
            await Context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Directors.RemoveRange(Context.Directors);
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldCreateDirector()
        {
            // Arrange
            var request = new DirectorDto(
                Id: null,
                FirstName: "New",
                LastName: "Director",
                BirthDate: new DateTime(1985, 2, 20));

            // Act
            var response = await Client.PostAsJsonAsync("directors", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var directorFromResponse = await response.Content.ReadFromJsonAsync<DirectorDto>();
            var directorId = new DirectorId(directorFromResponse!.Id!.Value);

            var directorFromDatabase = await Context.Directors.FirstOrDefaultAsync(x => x.Id == directorId);
            directorFromDatabase.Should().NotBeNull();
            directorFromDatabase!.FirstName.Should().Be("New");
            directorFromDatabase.LastName.Should().Be("Director");
            directorFromDatabase.BirthDate.Should().Be(new DateTime(1985, 2, 20));
        }

        [Fact]
        public async Task ShouldUpdateDirector()
        {
            // Arrange
            var newFirstName = "Updated";
            var newLastName = "Name";
            var newBirthDate = new DateTime(1980, 10, 10);

            var request = new DirectorDto(
                Id: _mainDirector.Id.Value,
                FirstName: newFirstName,
                LastName: newLastName,
                BirthDate: newBirthDate);

            // Act
            var response = await Client.PutAsJsonAsync("directors", request);

            // Assert the response is successful
            response.IsSuccessStatusCode.Should().BeTrue();

            // Fetch the director from the database again
            var updatedDirectorFromDatabase = await Context.Directors.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == _mainDirector.Id);
        
            // Ensure the director was updated
            updatedDirectorFromDatabase.Should().NotBeNull();
            updatedDirectorFromDatabase!.FirstName.Should().Be(newFirstName);
            updatedDirectorFromDatabase.LastName.Should().Be(newLastName);
            updatedDirectorFromDatabase.BirthDate.Should().Be(newBirthDate);
        }
        
        [Fact]
        public async Task ShouldDeleteDirector()
        {
            // Act
            var response = await Client.DeleteAsync($"directors/{_mainDirector.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var directorFromDatabase = await Context.Directors.FirstOrDefaultAsync(x => x.Id == _mainDirector.Id);
            directorFromDatabase.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnConflictWhenCreatingDirectorThatAlreadyExists()
        {
            // Arrange
            var request = new DirectorDto(
                Id: null,
                FirstName: _mainDirector.FirstName,
                LastName: _mainDirector.LastName,
                BirthDate: _mainDirector.BirthDate);

            // Act
            var existingDirector = await Context.Directors
                .FirstOrDefaultAsync(d => d.FirstName == request.FirstName && d.LastName == request.LastName && d.BirthDate == request.BirthDate);
            
            if (existingDirector != null)
            {
                var response = await Client.PostAsJsonAsync("directors", request);
                response.StatusCode.Should().Be(HttpStatusCode.Conflict);
                
                var directorsInDatabase = await Context.Directors.ToListAsync();
                directorsInDatabase.Count.Should().Be(1); 
            }
            else
            {
                var response = await Client.PostAsJsonAsync("directors", request);
                response.IsSuccessStatusCode.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentDirector()
        {
            var nonExistentDirectorId = Guid.NewGuid(); // Random GUID
            var request = new DirectorDto(
                Id: nonExistentDirectorId,
                FirstName: "Updated",
                LastName: "Name",
                BirthDate: new DateTime(1985, 5, 5));

            // Act
            var response = await Client.PutAsJsonAsync("directors", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentDirector()
        {
            // Arrange
            var nonExistentDirectorId = Guid.NewGuid(); 

            // Act
            var response = await Client.DeleteAsync($"directors/{nonExistentDirectorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            
            var directorsInDatabase = await Context.Directors.ToListAsync();
            directorsInDatabase.Count.Should().Be(1); 
        }
    }
}
