using Application.Directors.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class DirectorErrorHandler
{
    public static ObjectResult ToObjectResult(this DirectorException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                DirectorNotFoundException => StatusCodes.Status404NotFound,
                DirectorAlreadyExistsException => StatusCodes.Status409Conflict,
                DirectorUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Director error handler does not implemented")
            }
        };
    }
}