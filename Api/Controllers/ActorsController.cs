using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Actors.Commands;
using Domain.Actors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("actors")]
[ApiController]
public class ActorsController(ISender sender, IActorQueries actorQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ActorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await actorQueries.GetAll(cancellationToken);

        return entities.Select(ActorDto.FromDomainModel).ToList();
    }

    [HttpGet("{actorId:guid}")]
    public async Task<ActionResult<ActorDto>> Get([FromRoute] Guid actorId, CancellationToken cancellationToken)
    {
        var entity = await actorQueries.GetById(new ActorId(actorId), cancellationToken);

        return entity.Match<ActionResult<ActorDto>>(
            a => ActorDto.FromDomainModel(a),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<ActorDto>> Create([FromBody] ActorDto request, CancellationToken cancellationToken)
    {
        var input = new CreateActorCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ActorDto>>(
            a => ActorDto.FromDomainModel(a),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<ActorDto>> Update([FromBody] ActorDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateActorCommand
        {
            ActorId = request.Id!.Value,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ActorDto>>(
            actor => ActorDto.FromDomainModel(actor),
            e => e.ToObjectResult());
    }

    [HttpDelete("{actorId:guid}")]
    public async Task<ActionResult<ActorDto>> Delete([FromRoute] Guid actorId, CancellationToken cancellationToken)
    {
        var input = new DeleteActorCommand
        {
            ActorId = actorId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ActorDto>>(
            a => ActorDto.FromDomainModel(a),
            e => e.ToObjectResult());
    }
}
