using Api.Dtos;
using Application.Movies.Commands;
using Application.Movies.Exceptions;
using Application.Common.Interfaces.Queries;
using Microsoft.AspNetCore.Mvc;
using Domain.Movies;
using MediatR;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieQueries _movieQueries;
        private readonly IMediator _mediator;

        public MoviesController(IMovieQueries movieQueries, IMediator mediator)
        {
            _movieQueries = movieQueries;
            _mediator = mediator;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var movies = await _movieQueries.GetAll(cancellationToken);
                var movieDtos = movies.Select(movie => MovieDto.FromDomainModel(movie)).ToList();

                return Ok(movieDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        // GET: api/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var movieId = new MovieId(id);
            var movie = await _movieQueries.GetById(movieId, cancellationToken);
            return movie.Match<ActionResult<MovieDto>>(
                m => MovieDto.FromDomainModel(m), 
                () => NotFound());
        }
        
        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                movie => CreatedAtAction(nameof(GetById), new { id = movie.Id.Value }, MovieDto.FromDomainModel(movie)),
                exception => exception switch
                {
                    MovieAlreadyExistsException ex => Conflict(ex.Message),
                    MovieGenreNotFoundException ex => BadRequest(ex.Message),
                    _ => StatusCode(500, "An unexpected error occurred.")
                });
        }
        // PUT: api/movies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            if (id != command.MovieId)
                return BadRequest("Movie ID mismatch.");

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                movie => Ok(MovieDto.FromDomainModel(movie)),
                exception => exception switch
                {
                    MovieNotFoundException ex => NotFound(ex.Message),
                    _ => StatusCode(500, "An unexpected error occurred.")
                });
        }
        // DELETE: api/movies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieCommand { MovieId = id };
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                movie => Ok(MovieDto.FromDomainModel(movie)),
                exception => exception switch
                {
                    MovieNotFoundException ex => NotFound(ex.Message),
                    _ => StatusCode(500, "An unexpected error occurred.")
                });
        }
    }
}
