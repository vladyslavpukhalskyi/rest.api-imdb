using Application.Actors.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ActorErrorHandler
{
    public static ObjectResult ToObjectResult(this ActorException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                ActorNotFoundException => StatusCodes.Status404NotFound,
                ActorAlreadyExistsException => StatusCodes.Status409Conflict,
                ActorUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Actor error handler does not implement")
            }
        };
    }
}