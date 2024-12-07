using Api.Dtos;
using Api.Modules.Errors;
using Application.Directors.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Directors;
using Domain.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("directors")]
[ApiController]
public class DirectorsController(ISender sender, IDirectorQueries directorQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DirectorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await directorQueries.GetAll(cancellationToken);

        return entities.Select(DirectorDto.FromDomainModel).ToList();
    }

    [HttpGet("{directorId:guid}")]
    public async Task<ActionResult<DirectorDto>> Get([FromRoute] Guid directorId, CancellationToken cancellationToken)
    {
        var entity = await directorQueries.GetById(new DirectorId(directorId), cancellationToken);

        return entity.Match<ActionResult<DirectorDto>>(
            d => DirectorDto.FromDomainModel(d),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<DirectorDto>> Create([FromBody] DirectorDto request, CancellationToken cancellationToken)
    {
        var input = new CreateDirectorCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<DirectorDto>>(
            d => DirectorDto.FromDomainModel(d),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<DirectorDto>> Update([FromBody] DirectorDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateDirectorCommand
        {
            DirectorId = request.Id!.Value,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<DirectorDto>>(
            director => DirectorDto.FromDomainModel(director),
            e => e.ToObjectResult());
    }

    [HttpDelete("{directorId:guid}")]
    public async Task<ActionResult<DirectorDto>> Delete([FromRoute] Guid directorId, CancellationToken cancellationToken)
    {
        var input = new DeleteDirectorCommand
        {
            DirectorId = directorId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<DirectorDto>>(
            d => DirectorDto.FromDomainModel(d),
            e => e.ToObjectResult());
    }
}
