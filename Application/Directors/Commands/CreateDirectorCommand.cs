using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Directors.Exceptions;
using Domain.Directors;
using Domain.Movies;
using MediatR;

namespace Application.Directors.Commands;

public record CreateDirectorCommand : IRequest<Result<Director, DirectorException>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime BirthDate { get; init; }
}

public class CreateDirectorCommandHandler(
    IDirectorRepository directorRepository) 
    : IRequestHandler<CreateDirectorCommand, Result<Director, DirectorException>>
{
    public async Task<Result<Director, DirectorException>> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        var existingDirector = await directorRepository.GetByName($"{request.FirstName} {request.LastName}", cancellationToken);

        return await existingDirector.Match(
            d => Task.FromResult<Result<Director, DirectorException>>(new DirectorAlreadyExistsException(d.Id)),
            async () => await CreateEntity(request.FirstName, request.LastName, request.BirthDate, cancellationToken)
        );
    }

    private async Task<Result<Director, DirectorException>> CreateEntity(
        string firstName,
        string lastName,
        DateTime birthDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var director = Director.New(DirectorId.New(), firstName, lastName, birthDate);
            
            return await directorRepository.Add(director, cancellationToken);
        }
        catch (Exception exception)
        {
            return new DirectorUnknownException(DirectorId.Empty(), exception);
        }
    }
}