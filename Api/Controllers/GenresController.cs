using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Genres.Commands;
using Domain.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IGenreQueries _genreQueries;

        public GenresController(ISender sender, IGenreQueries genreQueries)
        {
            _sender = sender;
            _genreQueries = genreQueries;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GenreDto>>> GetAll(CancellationToken cancellationToken)
        {
            var genres = await _genreQueries.GetAll(cancellationToken);
            var genreDtos = genres.Select(GenreDto.FromDomainModel).ToList();
            return Ok(genreDtos);
        }

        [HttpGet("{genreId:guid}")]
        public async Task<ActionResult<GenreDto>> Get([FromRoute] Guid genreId, CancellationToken cancellationToken)
        {
            var genre = await _genreQueries.GetById(new GenreId(genreId), cancellationToken);

            return genre.Match<ActionResult<GenreDto>>(
                g => Ok(GenreDto.FromDomainModel(g)),
                () => NotFound());
        }

        [HttpPost]
        public async Task<ActionResult<GenreDto>> Create([FromBody] GenreDto request, CancellationToken cancellationToken)
        {
            var input = new CreateGenreCommand
            {
                Name = request.Name
            };

            var result = await _sender.Send(input, cancellationToken);

            return result.Match<ActionResult<GenreDto>>(
                g => CreatedAtAction(nameof(Get), new { genreId = g.Id.Value }, GenreDto.FromDomainModel(g)),
                e => e.ToObjectResult());
        }

        [HttpPut]
        public async Task<ActionResult<GenreDto>> Update([FromBody] GenreDto request, CancellationToken cancellationToken)
        {
            var input = new UpdateGenreCommand
            {
                GenreId = request.Id,
                Name = request.Name
            };

            var result = await _sender.Send(input, cancellationToken);

            return result.Match<ActionResult<GenreDto>>(
                g => Ok(GenreDto.FromDomainModel(g)),
                e => e.ToObjectResult());
        }

        [HttpDelete("{genreId:guid}")]
        public async Task<ActionResult<GenreDto>> Delete([FromRoute] Guid genreId, CancellationToken cancellationToken)
        {
            var input = new DeleteGenreCommand
            {
                GenreId = genreId
            };

            var result = await _sender.Send(input, cancellationToken);

            return result.Match<ActionResult<GenreDto>>(
                g => Ok(GenreDto.FromDomainModel(g)),
                e => e.ToObjectResult());
        }
    }
}
