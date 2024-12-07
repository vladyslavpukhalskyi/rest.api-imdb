using Application.Genres.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class GenreErrorHandler
{
    public static ObjectResult ToObjectResult(this GenreException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                GenreNotFoundException => StatusCodes.Status404NotFound,
                GenreAlreadyExistsException => StatusCodes.Status409Conflict,
                GenreUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Genre error handler not implemented")
            }
        };
    }
}