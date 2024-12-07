using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Directors.Exceptions;
using Domain.Directors;
using Domain.Movies;
using MediatR;

namespace Application.Directors.Commands;

public record DeleteDirectorCommand : IRequest<Result<Director, DirectorException>>
{
    public required Guid DirectorId { get; init; }
}

public class DeleteDirectorCommandHandler(
    IDirectorRepository directorRepository)
    : IRequestHandler<DeleteDirectorCommand, Result<Director, DirectorException>>
{
    public async Task<Result<Director, DirectorException>> Handle(
        DeleteDirectorCommand request,
        CancellationToken cancellationToken)
    {
        var directorId = new DirectorId(request.DirectorId);

        var existingDirector = await directorRepository.GetById(directorId, cancellationToken);

        return await existingDirector.Match<Task<Result<Director, DirectorException>>>( 
            async d => await DeleteEntity(d, cancellationToken),
            () => Task.FromResult<Result<Director, DirectorException>>(new DirectorNotFoundException(directorId)));
    }

    public async Task<Result<Director, DirectorException>> DeleteEntity(Director director, CancellationToken cancellationToken)
    {
        try
        {
            return await directorRepository.Delete(director, cancellationToken);
        }
        catch (Exception exception)
        {
            return new DirectorUnknownException(director.Id, exception);
        }
    }
}