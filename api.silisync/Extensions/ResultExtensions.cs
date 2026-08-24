using domain.silisync.Common.Results;
using domain.silisync.Enums;
using domain.silisync.Responses;
using Microsoft.AspNetCore.Mvc;

namespace api.silisync.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<TError>(this Result<TError> result)
        where TError : ResultError
    {
        return result.IsSuccess ? 
            new OkResult() 
            : ToProblemResult(result.Error!);
    }

    public static ObjectResult ToActionResult<T, TError>(this Result<T, TError> result)
        where TError : ResultError
    {
        if (!result.IsSuccess)
            return ToProblemResult(result.Error!);

        var response = new Response<T>(
            code: StatusCodes.Status200OK,
            data: result.Value,
            message: result.Message);

        return new OkObjectResult(response);
    }
    
    private static ObjectResult ToProblemResult(ResultError error)
    {
        var statusCode = error.Category switch
        {
            EErrorCategory.Validation => StatusCodes.Status400BadRequest,
            EErrorCategory.NotFound => StatusCodes.Status404NotFound,
            EErrorCategory.Conflict => StatusCodes.Status409Conflict,
            EErrorCategory.Unauthorized => StatusCodes.Status401Unauthorized,
            EErrorCategory.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var response = new Response<object>(
            code: statusCode,
            message: error.Message,
            errors: error.Details);
        
        return new ObjectResult(response);
    }
}