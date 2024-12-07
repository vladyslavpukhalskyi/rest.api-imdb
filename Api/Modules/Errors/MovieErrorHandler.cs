using Application.Movies.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class MovieErrorHandler
{
    public static ObjectResult ToObjectResult(this MovieException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                MovieNotFoundException or MovieDirectorNotFoundException or MovieGenreNotFoundException or MovieActorNotFoundException => StatusCodes.Status404NotFound,
                MovieAlreadyExistsException => StatusCodes.Status409Conflict,
                MovieUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Movie error handler does not implemented")
            }
        };
    }
}